using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlParser.Common;
using HtmlParser.Hash;

namespace HtmlParser.Lexer {
    
    public class HtmlLexer2 {

        private readonly char[] content = new char[1024*1024];
        private int length;
        private int index;

        private HtmlToken lastTag;
        private HtmlToken currentToken;
        private Func<bool> stateAction;

        public void Load(Stream stream, Encoding encoding) 
        {
            using (var reader = new StreamReader(stream, encoding)) {
                Load(reader);
            }
        }

        public void Load(TextReader reader) 
        {
            int startIndex = 0;
            int bytesRead = 1;
            while (bytesRead > 0) {
                bytesRead = reader.ReadBlock(content, startIndex, content.Length - startIndex);
                startIndex += bytesRead;
            }
            length = startIndex;
        }

        public void Load(string html) {
            using (var reader = new StringReader(html)) {
                Load(reader);
            }
        }


        public IEnumerable<HtmlToken> Parse() 
        {
            index = 0;
            stateAction = ParseToken;
            while (index < length) {
                if (!stateAction()) continue;
                yield return currentToken;
            }
        }


        #region State Actions
        private bool ParseToken() {
            SkipWhitespace();
            if ((index<length) && (content[index] == '<'))
                stateAction = ParseTagOrCommentOrDoctype;
            else
                stateAction = ParseTextValue;
            return false;
        }


        private bool ParseTextValue() {
            int startIndex = index;
            int lastNonspaceIndex = index;
            while ((index < length) && (content[index] != '<')) {
                if (!char.IsWhiteSpace(content[index])) {
                    lastNonspaceIndex = index;
                }
                index++;
            }
            stateAction = ParseToken;
            return FireValueToken(TokenType.Text, startIndex, lastNonspaceIndex + 1);
        }

        private bool ParseTagOrCommentOrDoctype() {
            // index should point to the char '<'
            var c = content[++index];
            switch (c) {
                case '!':
                    c = content[++index];
                    if ((c == '-') && (content[index + 1] == '-'))
                        stateAction = ParseComment;
                    else
                        stateAction = ParseDoctype;
                    return false;
                case '/':
                    stateAction = ParseClosedTag;
                    return false;
                default:
                    stateAction = ParseTag;
                    return false;
            }
        }

        private bool ParseComment() {
            // index should point to the next char after '<[!]--'
            index += 2;
            int startIndex = index;
            // Quirk mode
            GoToSequence("-->");
            var result = FireValueToken(TokenType.Comment, startIndex);
            index += 3;
            stateAction = ParseToken;
            return result;
        }

        private bool ParseDoctype() {
            // index should point to the char next to '<!'
            int startIndex = index;
            GoToChar('>');
            var result = FireValueToken(TokenType.Doctype, startIndex);
            index++;
            stateAction = ParseToken;
            return result;
        }

        private bool ParseClosedTag() {
            // index should point to the char '</'
            index++;
            var name = ParseQualifiedName();
            GoToChar('>');
            var result = FireToken(TokenType.CloseTag, name);
            index++;
            stateAction = ParseToken;
            return result;
        }

        private bool ParseTag() {
            // index should point to the char next to '<'
            var name = ParseQualifiedName();
            if (FireOpenTag(name)) {
                stateAction = ParseAttributesOrEndOpenTag;
                return true;
            } else {
                stateAction = ParseToken;  
                return false;
            }
        }

        private bool ParseAttributesOrEndOpenTag() {
            // index should point to to the char next to '<tagname'
            SkipWhitespace();
            var c = content[index];
            if ((c == '>') || (c == '/')) {
                stateAction = ParseEndBracket;
            } else {
                stateAction = ParseAttribute;
            }
            return false;
        }

        private bool ParseEndBracket() {
            stateAction = ParseToken;  // TODO : ???????
            var result = false;
            var c = content[index];
            if (c == '/') {
                if (content[index + 1] == '>') {
                    result = FireToken(TokenType.CloseTag, lastTag.Name);
                    index++;
                } else {
                    // Force to close current tag
                    GoToChar('>');
                }
            } else {
                var tagType = lastTag.GetTag(); 
                if (tagType == HtmlTag.Script) {
                    stateAction = ParseScript;
                } else if (tagType == HtmlTag.Style) {
                    stateAction = ParseStyle;
                }
            }
            index++;
            return result;
        }

        private bool ParseAttribute() {
            var attrName = ParseQualifiedName();
            if (attrName.Name.Length == 0) {
                index++; // Empty token, something wrong with html. Skip last char and try to continue parse
                return false;
            }

            var attrValue = default(StringSegment);
            SkipWhitespace();
            var c = content[index];
            if (c == '=') {
                index++;
                attrValue = ParseAttributeValue();
            }
            stateAction = ParseAttributesOrEndOpenTag;
            return FireToken(TokenType.Attribute, attrName, attrValue);
        }

        private bool ParseScript() {
            return ParseScriptOrStyle(TokenType.Script);
        }

        private bool ParseStyle() {
            return ParseScriptOrStyle(TokenType.Style);
        }

        private bool ParseScriptOrStyle(TokenType tokenType) {
            int startIndex = index;
            var tagType = lastTag.GetTag();
            var tagNameLength = lastTag.Name.Name.Length;
            GoToSequence("</");
            if (HtmlTagHash.GetTag(content, index + 2, tagNameLength) == tagType) {
                stateAction = ParseCloseLastTag;
                return FireValueToken(tokenType, startIndex);
            } else {
                index += "</".Length;
                return false;
            }
        }


        private bool ParseCloseLastTag() {
            FireToken(TokenType.CloseTag, lastTag.Name);
            index += lastTag.Name.Name.Length + "</>".Length;
            stateAction = ParseToken;
            return true;
        }

        //private void ParseStyle() {
        //    int startIndex = index;
        //    while (!IsEof()) {
        //        GoToSequence("</");
        //        if (HtmlTagHash.GetTag(content, index + 2, "style".Length) == HtmlTag.Script) {
        //            FireValueToken(TokenType.Style, startIndex);
        //            FireToken(TokenType.CloseTag, new QualifiedName(index + 2, "style".Length));
        //            index += "</style>".Length;
        //            isScript = false;
        //            break;
        //        }
        //        index += "</".Length;
        //    }
        //}


        private QualifiedName ParseQualifiedName() {
            int startIndex = index;
            int namespaceDelimeterIndex = -1;
            while (index < length) {
                var c = content[index];
                if (!char.IsLetterOrDigit(c)) {
                    if (c == ':') namespaceDelimeterIndex = index;
                    else if ((c!='-') && (c!='_')) break;
                }
                index++;
            }
            int len = index - startIndex;
            return new QualifiedName(startIndex, len, namespaceDelimeterIndex);
        }




        private StringSegment ParseAttributeValue()
        {
            SkipWhitespace();
            var c = content[index];
            int startIndex = index;
            if ((c == '\'') || (c == '"')) {
                ParseQuotedString();
                int len = index - startIndex - 2;
                return new StringSegment(startIndex + 1, len);
            }
            while (index < length) {
                c = content[index];
                if ((c=='>') || (c=='/') || (char.IsWhiteSpace(c))) break;
                index++;
            }
            return new StringSegment(startIndex, index-startIndex);
        }

        private void ParseQuotedString() {
            // TODO : analize escaped quotes
            var quote = content[index];
            index++;
            GoToChar(quote);
            index++;
        }

        #endregion State Actions


        private bool IsEof() {
            return (index>=length);
        }

        private void SkipWhitespace() {
            while ((index<length) && char.IsWhiteSpace(content[index])) index++;
        }

        private void GoToChar(char value) { 
            while ((index<length) && (content[index] != value)) index++;
        }

        private void GoToSequence(string sequence) {
            var len = sequence.Length;
            while (index < length) {
                GoToChar(sequence[0]);
                if ((content[index + 1] == sequence[1]) &&
                    ((len<3) || (content[index + 2] == sequence[2])) &&
                    ((len<4) || (content[index + 3] == sequence[3]))) 
                {
                    break;
                }
                index++;
            }
        }


        private bool FireToken(TokenType tokenType, QualifiedName name, StringSegment value = default(StringSegment)) {
            currentToken = new HtmlToken(tokenType, content, name, value);
            return true;
        }


        private bool FireValueToken(TokenType tokenType, int startIndex, int lastIndex = -1)
        {
            if (lastIndex == -1) lastIndex = index;
            if (lastIndex == startIndex) return false; // Empty token - Nothing to do
            var value = new StringSegment(startIndex, lastIndex - startIndex);
            return FireToken(tokenType, default(QualifiedName), value);
        }


        private bool FireOpenTag(QualifiedName name) {
            var result = FireToken(TokenType.OpenTag, name);
            var tokenType = currentToken.GetTag();
            lastTag = currentToken;
            return result;

            //if (tokenType == HtmlTag.Script) 
            //isStyle = (tokenType == HtmlTag.Style);
        }

    }
}

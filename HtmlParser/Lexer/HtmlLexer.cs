using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HtmlParser.Common;
using HtmlParser.Hash;

namespace HtmlParser.Lexer {
    
    public class HtmlLexer {

        private readonly char[] content = new char[1024*1024];
        private int length;
        private int index;
        private HtmlToken[] tokens = new HtmlToken[50];  // 29 - max count of attributes I met
        private int tokenCount;
        private bool isScript;
        private bool isStyle;

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
            isScript = false;
            isStyle = false;
            while (!IsEof()) {
                //var c = content[index];
                //if (c == 0) index+=2;
                index++;
                tokenCount = 0;

                if (isScript)
                    ParseScript();
                else if (isStyle)
                    ParseStyle();
                else
                    ParseToken();

                if (tokenCount==0) break;
                for (int i=0; i<tokenCount; i++) {
                    yield return tokens[i];
                }
            }
            yield break;
        }

        private void ParseScript() {
            int startIndex = index;
            while (!IsEof()) {
                GoToSequence("</");
                if (HtmlTagHash.GetTag(content, index + 2, "script".Length) == HtmlTag.Script) {
                    FireValueToken(TokenType.Script, startIndex);
                    FireToken(TokenType.CloseTag, new QualifiedName(index + 2, "script".Length));
                    index += "</script>".Length;
                    isScript = false;
                    break;
                }
                index += "</".Length;
            }
        }


        private void ParseStyle() {
            int startIndex = index;
            while (!IsEof()) {
                GoToSequence("</");
                if (HtmlTagHash.GetTag(content, index + 2, "style".Length) == HtmlTag.Script) {
                    FireValueToken(TokenType.Style, startIndex);
                    FireToken(TokenType.CloseTag, new QualifiedName(index + 2, "style".Length));
                    index += "</style>".Length;
                    isScript = false;
                    break;
                }
                index += "</".Length;
            }
        }

        private void ParseToken()  {
            SkipWhitespace();
            if (IsEof()) return;

            var c = content[index];
            if (c == '<')
                ParseTagOrCommentOrDoctype();
            else
                ParseTextValue();
        }

        private void ParseTagOrCommentOrDoctype()
        {
            // index should point to the char '<'
            var c = content[++index];
            switch (c) {
                case '!':
                    c = content[++index];
                    if ((c == '-') && (content[index + 1] == '-'))
                        ParseComment();
                    else
                        ParseDoctype();
                    break;
                case '/':
                    ParseClosedTag();
                    break;
                default:
                    ParseTag();
                    break;
            }
        }


        private void ParseDoctype() { 
            // index should point to the char next to '<!'
            int startIndex = index;
            GoToChar('>');
            FireValueToken(TokenType.Doctype, startIndex);
            index++;
        }

        private void ParseComment() {
            // index should point to the next char after '<[!]--'
            index += 2;
            int startIndex = index;
            // Quirk mode
            GoToSequence("-->");
            FireValueToken(TokenType.Comment, startIndex);
            index += 3;
        }

        private void ParseClosedTag() {
            // index should point to the char '</'
            index++;
            var name = ParseQualifiedName();
            GoToChar('>');
            FireToken(TokenType.CloseTag, name);
            index++;
        }

        private void ParseTag() {
            // index should point to the char next to '<'
            var name = ParseQualifiedName();
            FireOpenTag(name);
            ParseAttributesOrEndOpenTag(name);
        }

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

        private void ParseAttributesOrEndOpenTag(QualifiedName tagName) {
            // index should point to to the char next to '<tagname'
            while (index<length) {
                SkipWhitespace();
                var c = content[index];
                if ((c == '>') || (c == '/')) {
                    ParseEndBracket(tagName);
                    return;
                }
                ParseAttribute();
            }
        }

        private void ParseEndBracket(QualifiedName tagName) { 
            var c = content[index];
            if (c == '/') {
                if (content[index + 1] == '>') {
                    FireToken(TokenType.CloseTag, tagName);
                    index++;
                } else {
                    // Force to close current tag
                    GoToChar('>');
                }
            }
            index++;
        }

        private void ParseAttribute() {
            var attrName = ParseQualifiedName();
            if (attrName.Name.Length == 0) {
                index++; // Empty token, something wrong with html. Skip last char and try to continue parse
                return;
            }

            var attrValue = default(StringSegment);
            SkipWhitespace();
            var c = content[index];
            if (c == '=') {
                index++;
                attrValue = ParseAttributeValue();
            }
            FireToken(TokenType.Attribute, attrName, attrValue);
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


        private void ParseTextValue() {
            int startIndex = index;
            int lastNonspaceIndex = index;
            while ((index < length) && (content[index] != '<')) {
                if (!char.IsWhiteSpace(content[index])) {
                    lastNonspaceIndex = index;
                }
                index++;
            }
            FireValueToken(TokenType.Text, startIndex, lastNonspaceIndex+1);
        }

        private bool IsEof() {
            return (index>=length);
        }

        private void SkipWhitespace()
        {
            for (int i = index; (i < content.Length) && (i<length); i++) {
                var c = content[i];
                if ((c == ' ') || (c >= '\x0009' && c <= '\x000d') || c == '\x00a0' || c == '\x0085') continue;
                index = i;
                return;
            }
        }

        private void GoToChar(char value) { 
            //while ((index<length) && (content[index] != value)) index++;
            //for (int i = index; i < content.Length; i++) {
            //    var c = content[i];
            //    if (c != value) continue;
            //    index = i;
            //    return;
            //}
            while (index+4 < length) {
                if (content[index] == value) return;
                if (content[++index] == value) return;
                if (content[++index] == value) return;
                if (content[++index] == value) return;
            }
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


        private void FireToken(TokenType tokenType, QualifiedName name, StringSegment value = default(StringSegment)) {
            var token = new HtmlToken(tokenType, content, name, value);
            tokens[tokenCount++] = token;
        }


        private void FireValueToken(TokenType tokenType, int startIndex, int lastIndex = -1)
        {
            if (lastIndex == -1) lastIndex = index;
            if (lastIndex == startIndex) return; // Empty token - Nothing to do
            var value = new StringSegment(startIndex, lastIndex - startIndex);
            FireToken(tokenType, default(QualifiedName), value);
        }


        private void FireOpenTag(QualifiedName name) {
            FireToken(TokenType.OpenTag, name);
            var tokenType = tokens[tokenCount - 1].GetTag();
            isScript = (tokenType == HtmlTag.Script);
            isStyle = (tokenType == HtmlTag.Style);
        }

    }
}

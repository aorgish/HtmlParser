using HtmlParser.Common;
using HtmlParser.Hash;

namespace HtmlParser.Lexer {

    public enum TokenType { 
        OpenTag,
        CloseTag,
        Attribute,
        Text,
        Doctype,
        Comment,
        Script,
        Style
    }

    public struct HtmlToken {
        public TokenType TokenType;
        internal QualifiedName Name;
        private StringSegment Value;
        private char[] Source;
        private int hash;

        public HtmlToken(TokenType tokenType, char[] source, QualifiedName name, StringSegment value)  
        {
            TokenType = tokenType;
            Name = name;
            Value = value;
            Source = source;
            hash = -1;
        }

        public HtmlTag GetTag() {
            if (hash >= 0) return (HtmlTag)hash;
            hash = (int) HtmlTagHash.GetTag(Source, Name.Name.StartIndex, Name.Name.Length);
            return (HtmlTag) hash;
        }

        public HtmlAttribute GetAttribute() {
            if (hash >= 0) return (HtmlAttribute)hash;
            hash = (int)HtmlAttributeHash.GetAttribute(Source, Name.Name.StartIndex, Name.Name.Length);
            return (HtmlAttribute)hash;
        }


        public override string ToString()
        {
            var formats = new[] {
                @"Tag: <{0}>",            //  OpenTag
                @"Tag: </{0}>",           //  CloseTag    
                @"Attr: {0}=""{1}""",     //  Attribute
                @"Text: ""{1}""",         //  Text
                @"Doctype: ""{1}""",      //  Doctype
                @"Comment: <!--{1}-->",   //  Comment
                @"Script: {1}",           //  Script 
                @"Style: {1}"             //  Style 
            };

            return string.Format(formats[(int)TokenType], Name.Name.ToString(Source).ToLower(), Value.ToString(Source));
        }
    }

}

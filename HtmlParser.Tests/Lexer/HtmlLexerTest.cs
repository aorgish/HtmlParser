using System.Diagnostics;
using System.IO;
using System.Text;
using HtmlParser.Lexer;
using HtmlParser.Tests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HtmlParser.Tests.Lexer
{
    [TestClass]
    public class HtmlLexerTest {

        #region Tags

        [TestMethod]
        [TestCategory("Tags")]
        public void Parsing_Simple_Open_Tag() 
        {
            "<a>".ShouldReturn("Tag: <a>");
        }

        [TestMethod]
        [TestCategory("Tags")]
        public void Parsing_Self_Closing_Tag() 
        {
            "<br />".ShouldReturn("Tag: <br>", "Tag: </br>" );
        }

        [TestMethod]
        [TestCategory("Tags")]
        public void Parsing_Closed_Tag()
        {
            "</a>".ShouldReturn("Tag: </a>");
        }

        [TestMethod]
        [TestCategory("Tags")]
        public void Parsing_Nested_Tags()
        {
            "<p><span></span></p>"
            .ShouldReturn(
               "Tag: <p>", 
               "Tag: <span>", 
               "Tag: </span>", 
               "Tag: </p>"
             );
        }

        #endregion Tags

        #region Comments
        [TestMethod]
        [TestCategory("Comments")]
        public void Parsing_Simple_Comment()
        {
            "<!--  Test Comment  -->"
            .ShouldReturn(
                "Comment: <!--  Test Comment  -->"
            );
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void Parsing_Conditional_Comment()
        {
            "<!--[if !IE]> IE conditional <![Endif]-->"
            .ShouldReturn(
                 "Comment: <!--[if !IE]> IE conditional <![Endif]-->"
            );
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void Parsing_Wrapped_Comment() 
        {
            "<p><!-- Wrapped Comment--></p>"
            .ShouldReturn(
                 "Tag: <p>", 
                 "Comment: <!-- Wrapped Comment-->",
                 "Tag: </p>"
             );
        }

        [TestMethod]
        [TestCategory("Comments")]
        public void Parsing_Comment_With_Tags_Inside()
        {
            "<!-- <p>Commented para</p> -->"
            .ShouldReturn(
                "Comment: <!-- <p>Commented para</p> -->"
            );
        }

        #endregion Comments

        #region Attributes
        [TestMethod]
        [TestCategory("Attributes")]
        public void Parsing_Attribute_Without_Value() 
        {
            "<script async>"
            .ShouldReturn(
                @"*",
                @"Attr: async="""""
            );
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Parsing_Attribute_With_Value() 
        {
            @"<a href=""http://name.domain.com"">"
            .ShouldReturn(
                @"*",
                @"Attr: href=""http://name.domain.com"""
            );
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Parsing_Attribute_With_Signgle_Quoted_Value() 
        {
            @"<a href='http://name.domain.com'>"
            .ShouldReturn(
                @"*",  
                @"Attr: href=""http://name.domain.com"""
             );
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Parsing_Several_Attributes() 
        {
            @"<a href=""http://name.domain.com"" title=""Title"">"
            .ShouldReturn(
                @"*", 
                @"Attr: href=""http://name.domain.com""", @"Attr: title=""Title"""
            );
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Parsing_Style_Attribute() 
        {
            @"<div style=""background-image:url('/images/image.jpg'); height: 28px;width: 425px;"" >"
            .ShouldReturn(
                @"*", 
                @"Attr: style=""background-image:url('/images/image.jpg'); height: 28px;width: 425px;"""
            );
        }

        [TestMethod]
        [TestCategory("Attributes")]
        public void Parsing_Attribute_with_Special_Chars_Inside() 
        {
            @"<meta http-equiv=""Content-Type"">"
            .ShouldReturn(
                @"*",
                @"Attr: http-equiv=""Content-Type"""
            );

            // Custom attributes can include underscore
            @"<div display_row=""3"" >"
            .ShouldReturn(
                @"*",
                @"Attr: display_row=""3"""
            );
        }


        #endregion Attributes

        #region Text

        [TestMethod]
        [TestCategory("Text")]
        public void Parsing_Simple_Text() 
        {
            "Some Text".ShouldReturn(@"Text: ""Some Text""");
        }

        [TestMethod]
        [TestCategory("Text")]
        public void Parsing_Simple_Text_Inside_Tag() {
            "<p>Some Text</p>"
            .ShouldReturn(
                @"*",
                @"Text: ""Some Text""",
                @"*"
            );
        }

        [TestMethod]
        [TestCategory("Text")]
        public void Parsing_WhiteSpaces_Inside_Tag()
        {
            "<p>   \t \n \r   </p>"
            .ShouldReturn(
                "Tag: <p>",
                "Tag: </p>"
             );
        }

        [TestMethod]
        [TestCategory("Text")]
        public void Parsing_Text_Should_Return_Trimmed_Value() {
            "<p> \t  Some Text \n  </p>"
            .ShouldReturn(
                @"*",
                @"Text: ""Some Text""",
                @"*"
             );
        }


        [TestMethod]
        [TestCategory("Text")]
        public void Parsing_Text_Should_Decode_Entities() {
            Assert.Inconclusive("TODO");
            "<p>&lt;&gt;&amp;&nbsp;&quote;</p>"
            .ShouldReturn(
                @"*",
                @"<>& """,
                @"*"
             );
        }

        [TestMethod]
        [TestCategory("Text")]
        public void Parsing_Text_Should_Decode_Hex_Chars() {
            Assert.Inconclusive("TODO");
            "<p>&#x00AE;&#x00A9;</p>"
            .ShouldReturn(
                @"*",
                @"®©""",
                @"*"
             );
        }

        [TestMethod]
        [TestCategory("Text")]
        public void Parsing_Text_Should_Decode_Decimal_Chars() {
            Assert.Inconclusive("TODO");
            "<p>&#174;&#169;</p>"
            .ShouldReturn(
                @"*",
                @"®©""",
                @"*"
             );
        }

        #endregion Text

        #region Script
        [TestMethod]
        [TestCategory("Script")]
        public void Parsing_Simple_Script() {
            "<script> var value = window.document; </script>"
            .ShouldReturn(
                @"Tag: <script>",
                @"Script:  var value = window.document; ",
                @"Tag: </script>"
             );
        }

        [TestMethod]
        [TestCategory("Script")]
        public void Parsing_Script_with_Href() {
            @"<script href=""http://domain.com/script.js""></script>"
            .ShouldReturn(
                @"Tag: <script>",
                @"Attr: href=""http://domain.com/script.js""",
                @"Tag: </script>"
             );
        }

        [TestMethod]
        [TestCategory("Script")]
        public void Parsing_Script_with_Tags_Inside() {
//            @"<script>
//                  document.write('<style type=""text/css"">.tabber{display:none;}<\/style>');
//             </script>"
//            .ShouldReturn(
//                @"Tag: <script>",
//                @"Script: 
//                  document.write('<style type=""text/css"">.tabber{display:none;}<\/style>');
//             ",
//                @"Tag: </script>"
//             );



            @"<script>
                  this.AccessDatas = [{""data"":""<a href=\""/business/?SmRcid=ss_s_st_top_left\"" id=\""hsh_tx_setuzoku106no\"">法人インターネット</a></h3>\r\n""}];
             </script>"
            .ShouldReturn(
                @"Tag: <script>",
                @"Script: 
                  this.AccessDatas = [{""data"":""<a href=\""/business/?SmRcid=ss_s_st_top_left\"" id=\""hsh_tx_setuzoku106no\"">法人インターネット</a>""}];
             ",
                @"Tag: </script>"
             );



        }
        #endregion Script

        #region Style

        [TestMethod]
        [TestCategory("Style")]
        public void Parsing_Style_with_Tags_Inside() 
        {
            @"<style> 
                .sli1 {
                    filter: url(""data:image/svg+xml;utf8,<svg xmlns=\'http://www.w3.org/2000/svg\'><filter id=\'grayscale\'><feColorMatrix type=\'matrix\' values=\'0.3333 0.3333 0.3333 0 0 0.3333 0.3333 0.3333 0 0 0.3333 0.3333 0.3333 0 0 0 0 0 1 0\'/></filter></svg>#grayscale"");
                }
              </style>"
             .ShouldReturn(
                @"Tag: <style>",
                @"Style: .sli1 {
                    filter: url(""data:image/svg+xml;utf8,<svg xmlns=\'http://www.w3.org/2000/svg\'><filter id=\'grayscale\'><feColorMatrix type=\'matrix\' values=\'0.3333 0.3333 0.3333 0 0 0.3333 0.3333 0.3333 0 0 0.3333 0.3333 0.3333 0 0 0 0 0 1 0\'/></filter></svg>#grayscale"");
                }",
                @"Tag: </style>"
            );
        }



        #endregion Style


    }
}

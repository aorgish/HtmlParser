using System.Diagnostics;
using HtmlParser.Hash;
using HtmlParser.Tests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HtmlParser.Tests.Hash
{
    [TestClass]
    public class HtmlTagHashTest
    {
          string[] tagList = new string[201] {
      /*   0 */     null,       null,       null,       null,         null, 
      /*   5 */     null,       null,       null,       null,         null, 
      /*  10 */     null,       null,       null,       null,         null,
      /*  15 */     "noscript", "s",        null,       null,         null,       
      /*  20 */     null,       "script",   "strike",   "u",          "marquee",  
      /*  25 */     "basefont", "base",     "output",   "body",       "tt",
      /*  30 */     "summary",  "h6",       "noframes", "address",    "strong",
      /*  35 */     "article",  "h5",       "style",    "option",     "th",
      /*  40 */     "h4",       "title",    "button",   "blockquote", "optgroup",
      /*  45 */     "samp",     "time",     "hgroup",   "big",        "ol",
      /*  50 */     "ul",       "tfoot",    "dt",       "tbody",      "ins",
      /*  55 */     "iframe",   "video",    "br",       "hr",         "figure",
      /*  60 */     "menu",     "var",      "meter",    "datalist",   "audio",
      /*  65 */     "listing",  "tr",       "details",  "wbr",        "figcaption",
      /*  70 */     "html",     "aside",    "main",     "img",        "h3",
      /*  75 */     "h2",       "b",        "bdo",      "object",     "select",
      /*  80 */     "mark",     "code",     "h1",       "font",       "span",
      /*  85 */     "dl",       "cite",     "fieldset", "bgsound",    null,
      /*  90 */     "source",   "section",  "td",       null,         "acronym",
      /*  95 */     "rt",       null,       null,       "dir",        "p",
      /* 100 */     "table",    null,       "footer",   "dfn",        "frameset",
      /* 105 */     "frame",    "small",    "nav",      "progress",   "i",
      /* 110 */     "spacer",   "li",       "rp",       null,         "form",
      /* 115 */     "dd",       "menuitem", "nobr",     "canvas",     "header",
      /* 120 */     "applet",   "a",        "q",        "meta",       "blink",
      /* 125 */     "legend",   "sup",      "pre",      "map",        "em",
      /* 130 */     "abbr",     "bdi",      "xmp",      null,         "textarea",
      /* 135 */     null,       "thead",    "keygen",   "sub",        null, 
      /* 140 */     null,       null,       "input",    "plaintext",  "link",
      /* 145 */     "head",     null,       "track",    null,         "div",
      /* 150 */     "label",    "center",   "del",      "param",      null, 
      /* 155 */     null,       null,       null,       "colgroup",   null, 
      /* 160 */     null,       "data",     null,       null,         null,
      /* 165 */     "area",     null,       "ruby",     null,         null, 
      /* 170 */     null,       "kbd",      null,       null,         "col",
      /* 175 */     null,       null,       "isindex",  null,         null, 
      /* 180 */     null,       null,       "caption",  null,         null, 
      /* 185 */     null,       null,       null,       null,         null, 
      /* 190 */     null,       null,       null,       null,         null, 
      /* 195 */     null,       null,       null,       null,         null,
      /* 200 */     "embed"
        };



        [TestMethod]
        public void GetTokenHash_Should_Resolve_Hash()
        {
            HtmlTestExtentions.TestHash(tagList, x => (int)HtmlTagHash.GetTag(x.ToArray(), 0, x.Length));
        }

        [TestMethod]
        public void GetTokenHash_Should_Resolve_Hash_For_UppercaseTags()
        {
            HtmlTestExtentions.TestHash(tagList, x => (int)HtmlTagHash.GetTag(x.ToUpper().ToArray(), 0, x.Length));
        }

        [TestMethod]
        public void GetTokenHash_Should_Work_With_Actual_ArraySegment()
        {
            var actual = HtmlTagHash.GetTag("prefix title suffix".ToArray(), 7, 5);
            Assert.AreEqual(HtmlTag.Title, actual);
        }


        //[TestMethod]
        //public void GenerateEnumForTags() {
        //    for (int i = 0; i < tagList.Length; i++) {
        //        if (tagList[i] != null) {
        //            Debug.WriteLine("       {0} = {1},", new string(tagList[i].Take(1).Select(x => char.ToUpper(x)).Concat(tagList[i].Skip(1).Where(x => x != '-')).ToArray()), i);
        //        }
        //    }
        //}
    }
}

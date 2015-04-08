using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlParser.Hash;
using HtmlParser.Tests.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HtmlParser.Tests.Hash
{
    [TestClass]
    public class HtmlAttributeHashTest
    {
        readonly string[] attrList = new string[155] {
  /*   0 */     null,          null,             null,          "src",        null, 
  /*   5 */     null,          "cite",           null,          null,         null,
  /*  10 */     "title",       "target",         "class",       "alt",        "start",
  /*  15 */     "charset",     "shape",          "content",     "contenteditable", "span",
  /*  20 */     "step",        "type",           "cols",        "list",       "poster",
  /*  25 */     null,          "code",           null,          "accept",     "color",
  /*  30 */     "dir",         "accept-charset", "seamless",    "data",       "style",
  /*  35 */     "controls",    "srcdoc",         "enctype",     "async",      "pattern",
  /*  40 */     "scope",       "dirname",        "challenge",   "name",       "rel",
  /*  45 */     "open",        "autocomplete",   "selected",    "border",     "min",
  /*  50 */     null,          "for",            "headers",     "action",     null,
  /*  55 */     "colspan",     "autofocus",      "coords",      "ping",       "scoped",
  /*  60 */     "datetime",    "rows",           "lang",        "autoplay",   "codebase",
  /*  65 */     "placeholder", "bgcolor",        null,          "pubdate",    "loop",
  /*  70 */     "id",          "media",          "hidden",      "defer",      null,
  /*  75 */     "align",       null,             "srclang",     "form",       "radiogroup",
  /*  80 */     "tabindex",    "wrap",           "sizes",       "size",       "href",
  /*  85 */     "disabled",    "manifest",       "required",    "method",     "max",
  /*  90 */     "icon",        "value",          null,          "readonly",   "rowspan",
  /*  95 */     "summary",     "preload",        "label",       "reversed",   "contextmenu",
  /* 100 */     null,          null,             null,          "hreflang",   "draggable",
  /* 105 */     "sandbox",     "kind",           null,          "buffered",   "high",
  /* 110 */     null,          "ismap",          "usemap",      "accesskey",  null, 
  /* 115 */     null,          "itemprop",       null,          "http-equiv", "novalidate",
  /* 120 */     null,          null,             "multiple",    "checked",    null, 
  /* 125 */     null,          "height",         null,          "optimum",    "download",
  /* 130 */     null,          null,             "low",         null,         "maxlength",
  /* 135 */     "keytype",     null,             "spellcheck",  null,         null, 
  /* 140 */     null,          null,             "data-custom", null,         null,
  /* 145 */     null,          "dropzone",       null,          null,         "language",
  /* 150 */     null,          null,             null,          "default",    "width"
        };



        [TestMethod]
        public void GetTokenHash_Should_Resolve_Hash()
        {
            HtmlTestExtentions.TestHash(attrList, x => (int)HtmlAttributeHash.GetAttribute(x.ToArray(), 0, x.Length));
        }

        [TestMethod]
        public void GetTokenHash_Should_Resolve_Hash_For_UppercaseTags()
        {
            HtmlTestExtentions.TestHash(attrList, x => (int)HtmlAttributeHash.GetAttribute(x.ToUpper().ToArray(), 0, x.Length));
        }

        [TestMethod]
        public void GetTokenHash_Should_Work_With_Actual_ArraySegment()
        {
            var actual = HtmlAttributeHash.GetAttribute("prefix title suffix".ToArray(), 7, 5);
            Assert.AreEqual( HtmlAttribute.Title, actual);
        }

        //[TestMethod]
        //public void GenerateEnumForAttribute() {
        //    for (int i = 0; i < attrList.Length; i++) {
        //        if (attrList[i] != null)
        //        {
        //            Debug.WriteLine("       {0} = {1},",  new string(attrList[i].Take(1).Select(x=>char.ToUpper(x)).Concat(attrList[i].Skip(1).Where(x=>x!='-')).ToArray()) ,i);
        //        }
        //    }
        //}
    }
}

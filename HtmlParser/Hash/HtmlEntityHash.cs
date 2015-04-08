﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Hash {
    public class HtmlEntityHash {
        //public static KeyValuePair<char, string>[] Entities = {
//            ' ',"&nbsp;"  &#160;&#x00A0;no-break space = non-breaking space
//¡&iexcl;&#161;&#x00A1;inverted exclamation mark
//¢&cent;&#162;&#x00A2;cent sign
//£&pound;&#163;&#x00A3;pound sign
//¤&curren;&#164;&#x00A4;currency sign
//¥&yen;&#165;&#x00A5;yen sign = yuan sign
//¦&brvbar;&#166;&#x00A6;broken bar = broken vertical bar
//§&sect;&#167;&#x00A7;section sign
//¨&uml;&#168;&#x00A8;diaeresis = spacing diaeresis
//©&copy;&#169;&#x00A9;copyright sign
//ª&ordf;&#170;&#x00AA;feminine ordinal indicator
//«&laquo;&#171;&#x00AB;left-pointing double angle quotation mark = left pointing guillemet
//¬&not;&#172;&#x00AC;not sign = angled dash
//&shy;&#173;&#x00AD;soft hyphen = discretionary hyphen
//®&reg;&#174;&#x00AE;registered sign = registered trade mark sign
//¯&macr;&#175;&#x00AF;macron = spacing macron = overline = APL overbar
//°&deg;&#176;&#x00B0;degree sign
//±&plusmn;&#177;&#x00B1;plus-minus sign = plus-or-minus sign
//²&sup2;&#178;&#x00B2;superscript two = superscript digit two = squared
//³&sup3;&#179;&#x00B3;superscript three = superscript digit three = cubed
//´&acute;&#180;&#x00B4;acute accent = spacing acute
//µ&micro;&#181;&#x00B5;micro sign
//¶&para;&#182;&#x00B6;pilcrow sign = paragraph sign
//·&middot;&#183;&#x00B7;middle dot = Georgian comma = Greek middle dot
//¸&cedil;&#184;&#x00B8;cedilla = spacing cedilla
//¹&sup1;&#185;&#x00B9;superscript one = superscript digit one
//º&ordm;&#186;&#x00BA;masculine ordinal indicator
//»&raquo;&#187;&#x00BB;right-pointing double angle quotation mark = right pointing guillemet
//¼&frac14;&#188;&#x00BC;vulgar fraction one quarter = fraction one quarter
//½&frac12;&#189;&#x00BD;vulgar fraction one half = fraction one half
//¾&frac34;&#190;&#x00BE;vulgar fraction three quarters = fraction three quarters
//¿&iquest;&#191;&#x00BF;inverted question mark = turned question mark
//À&Agrave;&#192;&#x00C0;latin capital letter A with grave = latin capital letter A grave
//Á&Aacute;&#193;&#x00C1;latin capital letter A with acute
//Â&Acirc;&#194;&#x00C2;latin capital letter A with circumflex
//Ã&Atilde;&#195;&#x00C3;latin capital letter A with tilde
//Ä&Auml;&#196;&#x00C4;latin capital letter A with diaeresis
//Å&Aring;&#197;&#x00C5;latin capital letter A with ring above = latin capital letter A ring
//Æ&AElig;&#198;&#x00C6;latin capital letter AE = latin capital ligature AE
//Ç&Ccedil;&#199;&#x00C7;latin capital letter C with cedilla
//È&Egrave;&#200;&#x00C8;latin capital letter E with grave
//É&Eacute;&#201;&#x00C9;latin capital letter E with acute
//Ê&Ecirc;&#202;&#x00CA;latin capital letter E with circumflex
//Ë&Euml;&#203;&#x00CB;latin capital letter E with diaeresis
//Ì&Igrave;&#204;&#x00CC;latin capital letter I with grave
//Í&Iacute;&#205;&#x00CD;latin capital letter I with acute
//Î&Icirc;&#206;&#x00CE;latin capital letter I with circumflex
//Ï&Iuml;&#207;&#x00CF;latin capital letter I with diaeresis
//Ð&ETH;&#208;&#x00D0;latin capital letter ETH
//Ñ&Ntilde;&#209;&#x00D1;latin capital letter N with tilde
//Ò&Ograve;&#210;&#x00D2;latin capital letter O with grave
//Ó&Oacute;&#211;&#x00D3;latin capital letter O with acute
//Ô&Ocirc;&#212;&#x00D4;latin capital letter O with circumflex
//Õ&Otilde;&#213;&#x00D5;latin capital letter O with tilde
//Ö&Ouml;&#214;&#x00D6;latin capital letter O with diaeresis
//×&times;&#215;&#x00D7;multiplication sign
//Ø&Oslash;&#216;&#x00D8;latin capital letter O with stroke = latin capital letter O slash
//Ù&Ugrave;&#217;&#x00D9;latin capital letter U with grave
//Ú&Uacute;&#218;&#x00DA;latin capital letter U with acute
//Û&Ucirc;&#219;&#x00DB;latin capital letter U with circumflex
//Ü&Uuml;&#220;&#x00DC;latin capital letter U with diaeresis
//Ý&Yacute;&#221;&#x00DD;latin capital letter Y with acute
//Þ&THORN;&#222;&#x00DE;latin capital letter THORN
//ß&szlig;&#223;&#x00DF;latin small letter sharp s = ess-zed
//à&agrave;&#224;&#x00E0;latin small letter a with grave = latin small letter a grave
//á&aacute;&#225;&#x00E1;latin small letter a with acute
//â&acirc;&#226;&#x00E2;latin small letter a with circumflex
//ã&atilde;&#227;&#x00E3;latin small letter a with tilde
//ä&auml;&#228;&#x00E4;latin small letter a with diaeresis
//å&aring;&#229;&#x00E5;latin small letter a with ring above = latin small letter a ring
//æ&aelig;&#230;&#x00E6;latin small letter ae = latin small ligature ae
//ç&ccedil;&#231;&#x00E7;latin small letter c with cedilla
//è&egrave;&#232;&#x00E8;latin small letter e with grave
//é&eacute;&#233;&#x00E9;latin small letter e with acute
//ê&ecirc;&#234;&#x00EA;latin small letter e with circumflex
//ë&euml;&#235;&#x00EB;latin small letter e with diaeresis
//ì&igrave;&#236;&#x00EC;latin small letter i with grave
//í&iacute;&#237;&#x00ED;latin small letter i with acute
//î&icirc;&#238;&#x00EE;latin small letter i with circumflex
//ï&iuml;&#239;&#x00EF;latin small letter i with diaeresis
//ð&eth;&#240;&#x00F0;latin small letter eth
//ñ&ntilde;&#241;&#x00F1;latin small letter n with tilde
//ò&ograve;&#242;&#x00F2;latin small letter o with grave
//ó&oacute;&#243;&#x00F3;latin small letter o with acute
//ô&ocirc;&#244;&#x00F4;latin small letter o with circumflex
//õ&otilde;&#245;&#x00F5;latin small letter o with tilde
//ö&ouml;&#246;&#x00F6;latin small letter o with diaeresis
//÷&divide;&#247;&#x00F7;division sign
//ø&oslash;&#248;&#x00F8;latin small letter o with stroke, = latin small letter o slash
//ù&ugrave;&#249;&#x00F9;latin small letter u with grave
//ú&uacute;&#250;&#x00FA;latin small letter u with acute
//û&ucirc;&#251;&#x00FB;latin small letter u with circumflex
//ü&uuml;&#252;&#x00FC;latin small letter u with diaeresis
//ý&yacute;&#253;&#x00FD;latin small letter y with acute
//þ&thorn;&#254;&#x00FE;latin small letter thorn
//ÿ&yuml;&#255;&#x00FF;latin small letter y with diaeresis
//"&quot;&#34;&#x0022;quotation mark
//&&amp;&#38;&#x0026;ampersand
//<&lt;&#60;&#x003C;less-than sign
//>&gt;&#62;&#x003E;greater-than sign
//'&apos;&#39;&#x0027;apostrophe = APL quote
//Œ&OElig;&#338;&#x0152;latin capital ligature OE
//œ&oelig;&#339;&#x0153;latin small ligature oe
//Š&Scaron;&#352;&#x0160;latin capital letter S with caron
//š&scaron;&#353;&#x0161;latin small letter s with caron
//Ÿ&Yuml;&#376;&#x0178;latin capital letter Y with diaeresis
//ˆ&circ;&#710;&#x02C6;modifier letter circumflex accent
//˜&tilde;&#732;&#x02DC;small tilde
// &ensp;&#8194;&#x2002;en space
// &emsp;&#8195;&#x2003;em space
// &thinsp;&#8201;&#x2009;thin space
//‌&zwnj;&#8204;&#x200C;zero width non-joiner
//‍&zwj;&#8205;&#x200D;zero width joiner
//‎&lrm;&#8206;&#x200E;left-to-right mark
//‏&rlm;&#8207;&#x200F;right-to-left mark
//–&ndash;&#8211;&#x2013;en dash
//—&mdash;&#8212;&#x2014;em dash
//‘&lsquo;&#8216;&#x2018;left single quotation mark
//’&rsquo;&#8217;&#x2019;right single quotation mark
//‚&sbquo;&#8218;&#x201A;single low-9 quotation mark
//“&ldquo;&#8220;&#x201C;left double quotation mark
//”&rdquo;&#8221;&#x201D;right double quotation mark
//„&bdquo;&#8222;&#x201E;double low-9 quotation mark
//†&dagger;&#8224;&#x2020;dagger
//‡&Dagger;&#8225;&#x2021;double dagger
//‰&permil;&#8240;&#x2030;per mille sign
//‹&lsaquo;&#8249;&#x2039;single left-pointing angle quotation mark
//›&rsaquo;&#8250;&#x203A;single right-pointing angle quotation mark
//€&euro;&#8364;&#x20AC;euro sign
//ƒ&fnof;&#402;&#x0192;latin small letter f with hook = function = florin
//Γ&Gamma;&#915;&#x0393;greek capital letter gamma
//Δ&Delta;&#916;&#x0394;greek capital letter delta
//Θ&Theta;&#920;&#x0398;greek capital letter theta
//Λ&Lambda;&#923;&#x039B;greek capital letter lamda
//Ξ&Xi;&#926;&#x039E;greek capital letter xi
//Π&Pi;&#928;&#x03A0;greek capital letter pi
//Σ&Sigma;&#931;&#x03A3;greek capital letter sigma
//Υ&Upsilon;&#933;&#x03A5;greek capital letter upsilon
//Φ&Phi;&#934;&#x03A6;greek capital letter phi
//Ψ&Psi;&#936;&#x03A8;greek capital letter psi
//Ω&Omega;&#937;&#x03A9;greek capital letter omega
//α&alpha;&#945;&#x03B1;greek small letter alpha
//β&beta;&#946;&#x03B2;greek small letter beta
//γ&gamma;&#947;&#x03B3;greek small letter gamma
//δ&delta;&#948;&#x03B4;greek small letter delta
//ε&epsilon;&#949;&#x03B5;greek small letter epsilon
//ζ&zeta;&#950;&#x03B6;greek small letter zeta
//η&eta;&#951;&#x03B7;greek small letter eta
//θ&theta;&#952;&#x03B8;greek small letter theta
//ι&iota;&#953;&#x03B9;greek small letter iota
//κ&kappa;&#954;&#x03BA;greek small letter kappa
//λ&lambda;&#955;&#x03BB;greek small letter lamda
//μ&mu;&#956;&#x03BC;greek small letter mu
//ν&nu;&#957;&#x03BD;greek small letter nu
//ξ&xi;&#958;&#x03BE;greek small letter xi
//ο&omicron;&#959;&#x03BF;greek small letter omicron
//π&pi;&#960;&#x03C0;greek small letter pi
//ρ&rho;&#961;&#x03C1;greek small letter rho
//ς&sigmaf;&#962;&#x03C2;greek small letter final sigma
//σ&sigma;&#963;&#x03C3;greek small letter sigma
//τ&tau;&#964;&#x03C4;greek small letter tau
//υ&upsilon;&#965;&#x03C5;greek small letter upsilon
//φ&phi;&#966;&#x03C6;greek small letter phi
//χ&chi;&#967;&#x03C7;greek small letter chi
//ψ&psi;&#968;&#x03C8;greek small letter psi
//ω&omega;&#969;&#x03C9;greek small letter omega
//ϑ&thetasym;&#977;&#x03D1;greek theta symbol
//ϒ&upsih;&#978;&#x03D2;greek upsilon with hook symbol
//ϖ&piv;&#982;&#x03D6;greek pi symbol
//•&bull;&#8226;&#x2022;bullet = black small circle
//…&hellip;&#8230;&#x2026;horizontal ellipsis = three dot leader
//′&prime;&#8242;&#x2032;prime = minutes = feet
//″&Prime;&#8243;&#x2033;double prime = seconds = inches
//‾&oline;&#8254;&#x203E;overline = spacing overscore
//⁄&frasl;&#8260;&#x2044;fraction slash
//℘&weierp;&#8472;&#x2118;script capital P = power set = Weierstrass p
//ℑ&image;&#8465;&#x2111;black-letter capital I = imaginary part
//ℜ&real;&#8476;&#x211C;black-letter capital R = real part symbol
//™&trade;&#8482;&#x2122;trade mark sign
//ℵ&alefsym;&#8501;&#x2135;alef symbol = first transfinite cardinal
//←&larr;&#8592;&#x2190;leftwards arrow
//↑&uarr;&#8593;&#x2191;upwards arrow
//→&rarr;&#8594;&#x2192;rightwards arrow
//↓&darr;&#8595;&#x2193;downwards arrow
//↔&harr;&#8596;&#x2194;left right arrow
//↵&crarr;&#8629;&#x21B5;downwards arrow with corner leftwards = carriage return
//⇐&lArr;&#8656;&#x21D0;leftwards double arrow
//⇑&uArr;&#8657;&#x21D1;upwards double arrow
//⇒&rArr;&#8658;&#x21D2;rightwards double arrow
//⇓&dArr;&#8659;&#x21D3;downwards double arrow
//⇔&hArr;&#8660;&#x21D4;left right double arrow
//∀&forall;&#8704;&#x2200;for all
//∂&part;&#8706;&#x2202;partial differential
//∃&exist;&#8707;&#x2203;there exists
//∅&empty;&#8709;&#x2205;empty set = null set
//∇&nabla;&#8711;&#x2207;nabla = backward difference
//∈&isin;&#8712;&#x2208;element of
//∉&notin;&#8713;&#x2209;not an element of
//∋&ni;&#8715;&#x220B;contains as member
//∏&prod;&#8719;&#x220F;n-ary product = product sign
//∑&sum;&#8721;&#x2211;n-ary summation
//−&minus;&#8722;&#x2212;minus sign
//∗&lowast;&#8727;&#x2217;asterisk operator
//√&radic;&#8730;&#x221A;square root = radical sign
//∝&prop;&#8733;&#x221D;proportional to
//∞&infin;&#8734;&#x221E;infinity
//∠&ang;&#8736;&#x2220;angle
//∧&and;&#8743;&#x2227;logical and = wedge
//∨&or;&#8744;&#x2228;logical or = vee
//∩&cap;&#8745;&#x2229;intersection = cap
//∪&cup;&#8746;&#x222A;union = cup
//∫&int;&#8747;&#x222B;integral
//∴&there4;&#8756;&#x2234;therefore
//∼&sim;&#8764;&#x223C;tilde operator = varies with = similar to
//≅&cong;&#8773;&#x2245;approximately equal to
//≈&asymp;&#8776;&#x2248;almost equal to = asymptotic to
//≠&ne;&#8800;&#x2260;not equal to
//≡&equiv;&#8801;&#x2261;identical to
//≤&le;&#8804;&#x2264;less-than or equal to
//≥&ge;&#8805;&#x2265;greater-than or equal to
//⊂&sub;&#8834;&#x2282;subset of
//⊃&sup;&#8835;&#x2283;superset of
//⊄&nsub;&#8836;&#x2284;not a subset of
//⊆&sube;&#8838;&#x2286;subset of or equal to
//⊇&supe;&#8839;&#x2287;superset of or equal to
//⊕&oplus;&#8853;&#x2295;circled plus = direct sum
//⊗&otimes;&#8855;&#x2297;circled times = vector product
//⊥&perp;&#8869;&#x22A5;up tack = orthogonal to = perpendicular
//⋅&sdot;&#8901;&#x22C5;dot operator
//⌈&lceil;&#8968;&#x2308;left ceiling = APL upstile
//⌉&rceil;&#8969;&#x2309;right ceiling
//⌊&lfloor;&#8970;&#x230A;left floor = APL downstile
//⌋&rfloor;&#8971;&#x230B;right floor
//〈&lang;&#9001;&#x2329;left-pointing angle bracket = bra
//〉&rang;&#9002;&#x232A;right-pointing angle bracket = ket
//◊&loz;&#9674;&#x25CA;lozenge
//♠&spades;&#9824;&#x2660;black spade suit
//♣&clubs;&#9827;&#x2663;black club suit = shamrock
//♥&hearts;&#9829;&#x2665;black heart suit = valentine
//♦&diams;&#9830;&#x2666;black diamond suit
    }
}

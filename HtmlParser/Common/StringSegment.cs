using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HtmlParser.Common {

    public struct StringSegment {
        public int StartIndex;
        public int Length;

        public StringSegment(int startIndex, int length) {
            StartIndex = startIndex;
            Length = length;
        }

        public string ToString(char[] source) {
            return Length == 0 ? string.Empty
                             : new string(source, StartIndex, Length);
        }
    }

    public struct QualifiedName {
        public StringSegment Namespace;
        public StringSegment Name;

        public QualifiedName(int startIndex, int length, int delimeterIndex = -1) {
            if (delimeterIndex == -1) {
                Name = new StringSegment(startIndex, length);
                Namespace = default(StringSegment);
            } else {
                Namespace = new StringSegment(startIndex, delimeterIndex - startIndex - 1);
                Name = new StringSegment(delimeterIndex + 1, length - (delimeterIndex - startIndex));
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLVidalTranspose.Core
{
    public class CommonParserToText
    {
        public static DefTextPreTypeTransposer DateFormat(string format) => new DefTextTransposer<DateTime>(a => a.ToString(format));

        public  static DefTextPreTypeTransposer DateNullableFormat(string format) => new DefTextTransposer<DateTime?>(a => !a.HasValue ? string.Empty : a.Value.ToString(format));

        public readonly static DefTextPreTypeTransposer IntNullableFormat = new DefTextTransposer<int?>(a => !a.HasValue ? string.Empty : a.Value.ToString());

        public readonly static DefTextPreTypeTransposer BoolFormat = new DefTextTransposer<bool>(a => a ? "1" : "0");

        public readonly static DefTextPreTypeTransposer BoolNullableFormat = new DefTextTransposer<bool?>(a => a.HasValue ? (a.Value ? "1" : "0") : string.Empty);

        public readonly static DefTextPreTypeTransposer StringNullFormat = new DefTextTransposer<string>(a => a == null ? string.Empty : a);
    }
}

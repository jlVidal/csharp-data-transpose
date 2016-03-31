using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLVidalTranspose.Core
{
    public class CommonTextTransposition
    {
        public static DefPreTransposeBase<string> ReplaceStringTransform(string toReplace, string changeTo, string propTarget = null)
        {
            if (propTarget == null)
                return new DefPreTransposeBase<string>(a => (a ?? string.Empty).Replace(toReplace, changeTo));

            return new DefPreTransposeBase<string>(propTarget, a => (a ?? string.Empty).Replace(toReplace, changeTo));
        }
        public static DefTextSize GetSizeTransform(int size, string propTarget = null)
        {
            if (propTarget == null)
                return new DefTextSize(size);

            return new DefTextSize(propTarget, size);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLVidalTranspose.Core
{
    public class DefTextSize : DefPreTransposeBase<string>
    {
        private int _size;

        public int Size
        {
            get { return _size; }
        }

        public DefTextSize(int size)
            : this(string.Empty, size)
        {

        }
        public DefTextSize(string prop, int size)
            : base(prop, a => a.Length > size ? new string((a ?? string.Empty).Take(size).ToArray()) : a)
        {
            this._size = size;
        }
    }
}

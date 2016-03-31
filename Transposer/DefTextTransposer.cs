using JLVidalTranspose.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLVidalTranspose.Core
{
    public class DefTextPosTransposer : DefTextTransposer<string>
    {
        public DefTextPosTransposer(Func<string, string> parser)
            : base(parser)
        {

        }

    }
    public class DefTextTransposer<T> : DefTextPreTypeTransposer
    {
        public DefTextTransposer(Func<T, string> parser)
            : base(typeof(T), GenericToObjectFunc(parser))
        {

        }

        private static Func<object, string> GenericToObjectFunc(Func<T, string> parser)
        {
            return (arg =>
                {
                    return  parser(arg.ConvertOrDefaultOrEmpty<T>());
                });
        }

    }
    public class DefTextPreTypeTransposer
    {
        private readonly Func<object, string> _func;
        public DefTextPreTypeTransposer(Type target, Func<object, string> func)
        {
            this.TargetType = target;
            this._func = func;
        }
        public Type TargetType { get; set; }

        public string DoTranspose(object value)
        {
            return _func(value);
        }

    }
}

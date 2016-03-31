using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JLVidalTranspose.Helper;
using System.Linq.Expressions;

namespace JLVidalTranspose.Core
{

    public class DefPreTransposeBase<T> : DefTransposeInit<T, T>
    {
        public DefPreTransposeBase(Func<T, T> func)
            : base(func)
        {

        }

        public DefPreTransposeBase(string propName)
            : base(propName)
        {

        }
        public DefPreTransposeBase(string propName, Func<T, T> func)
            : base(propName, func)
        {

        }

        public override bool SafeConversion
        {
            get
            {
                return true;
            }
        }

        protected override T ConvertArgBeforeTransform(object value)
        {
            return value.ConvertOrDefault<T>();
        }
    }

    public class DefPreTransposeBaseFor<TObj, TTransform> : DefPreTransposeBase<TTransform>
    {

        public DefPreTransposeBaseFor(Expression<Func<TObj, object>> propName)
            : base(ExtGen.GetMemName(propName))
        {

        }
        public DefPreTransposeBaseFor(Func<TTransform, TTransform> func)
            : base(func)
        {

        }

        public DefPreTransposeBaseFor(string propName, Func<TTransform, TTransform> func)
            : base(propName, func)
        {

        }

        public DefPreTransposeBaseFor(Expression<Func<TObj, object>> propName, Func<TTransform, TTransform> func)
            : base(ExtGen.GetMemName(propName), func)
        {

        }

    }


    public class DefPreTransformDefault : DefTransposeInit<object, string>
    {
        public DefPreTransformDefault(Func<object, string> func)
            : base(func)
        {

        }
        public DefPreTransformDefault(string propName)
            : base(propName)
        {

        }
        public DefPreTransformDefault(string propName, Func<object, string> func)
            : base(propName, func)
        {

        }

        public override bool SafeConversion
        {
            get
            {
                return true;
            }
        }



        protected override object ConvertArgBeforeTransform(object value)
        {
            return value;
        }

    }


    public class DefPreTransformDefaultFor<TObj> : DefPreTransformDefault
    {
        public DefPreTransformDefaultFor(Func<object, string> func)
            : base(func)
        {

        }

        public DefPreTransformDefaultFor(Expression<Func<TObj, object>> propName)
            : base(ExtGen.GetMemName(propName))
        {

        }

        public DefPreTransformDefaultFor(Expression<Func<TObj, object>> propName, Func<object, string> func)
            : this(ExtGen.GetMemName(propName), func)
        {

        }

        public DefPreTransformDefaultFor(string propName, Func<object, string> func)
            : base(propName, func)
        {

        }

        //private static Func<object, string> GenericToObjectToStringFunc(Func<T, T> parser)
        //{
        //    return arg =>
        //    {
        //        var argValue = arg.ConvertOrDefault<T>();
        //        var parseResult = parser(argValue);
        //        if (parseResult == null)
        //            return string.Empty;

        //        return parseResult.ToString();
        //    };
        //}

    }


}

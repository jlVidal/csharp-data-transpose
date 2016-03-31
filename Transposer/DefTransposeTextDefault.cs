using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JLVidalTranspose.Core;
using JLVidalTranspose.Helper;

namespace JLVidalTranspose
{
    public class DefTransposeTextDefault<TArg> : DefTransposeInit<TArg, string>
    {
        protected DefTransposeTextDefault()
        {

        }

        public DefTransposeTextDefault(Func<TArg, string> func)
            : base(func)
        {
        }

        public DefTransposeTextDefault(string prop, Func<TArg, string> func)
            : base(prop, func)
        {


        }

        public override bool SafeConversion
        {
            get { return true; }
        }

        protected override TArg ConvertArgBeforeTransform(object value)
        {
            return value.ConvertOrDefault<TArg>();
        }
    }

    public class DefPosTransposition : DefTransposeTextDefault<string>
    {
        protected DefPosTransposition()
        {

        }

        public DefPosTransposition(Func<string, string> func)
            : base(func)
        {
        }

        public DefPosTransposition(string prop, Func<string, string> func)
            : base(prop, func)
        {

        }

        public DefPosTransposition Apply(Func<string, string> other)
        {
            base.Override(other);
            return this;
        }
    }

    public class DefPosTransformFor<T> : DefPosTransposition
    {
        public DefPosTransformFor(Func<string> propertyNameAccessor, Func<string, string> func)
            : base(func)
        {
            this.For(propertyNameAccessor);
        }

        public DefPosTransformFor(Expression<Func<T, object>> propertyNameAccessor, Func<string, string> func)
            : base(func)
        {
            this.For(() => Helper.ExtGen.GetMemName<T>(propertyNameAccessor));
        }
        public DefPosTransformFor(Expression<Func<T, object>> propertyNameAccessor)
        {
            this.For(() => Helper.ExtGen.GetMemName<T>(propertyNameAccessor));
        }
        public DefPosTransformFor(string prop)
        {
            this.For(() => prop);
        }
        public DefPosTransformFor(string prop, Func<string, string> func)
            : base(prop, func)
        {

        }

        public DefPosTransformFor(Func<string, string> func)
            : base(func)
        {

        }

        public DefPosTransformFor<T> For(Expression<Func<T, object>> propertyNameAccessor)
        {
            this.For(() => Helper.ExtGen.GetMemName(propertyNameAccessor));
            return this;
        }
    }

    public static class DefinitionPosTransformHelper
    {
        public static DefPosTransformFor<T> UseTooWith<T>(this DefPosTransformFor<T> current, Func<string, string> other)
        {
            var copy = current.CopyWith(other);
            return new DefPosTransformFor<T>(() => current.TargetProperty, (a) => copy.DoTranspose(current.DoTranspose(a)));
        }

        public static DefPosTransformFor<T> UseWith<T>(this DefPosTransformFor<T> current, Func<string, string> other)
        {
            current.Apply(other);
            return current;
        }

    }
}

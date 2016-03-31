using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLVidalTranspose.Core
{
    public abstract class DefTransposeInit<TArg, TRes> : IDefTranspose
    {
        public abstract bool SafeConversion { get; }

        private Func<TArg, TRes> _func;
        protected Func<TArg, TRes> Func
        {
            get { return _func; }
        }

        private Func<string> _targetPropertyAccessor;

        public string TargetProperty
        {
            get { return _targetPropertyAccessor(); }
            set { _targetPropertyAccessor = () => value ?? string.Empty; }
        }

        protected DefTransposeInit()
        {

        }

        public DefTransposeInit(Func<TArg, TRes> func)
        {
            this._func = func;
            this._targetPropertyAccessor = () => string.Empty;
        }
        public DefTransposeInit(string prop)
        {
            this._targetPropertyAccessor = () => string.Empty;
        }
        public DefTransposeInit(string prop, Func<TArg, TRes> func)
        {
            this._targetPropertyAccessor = () => prop;
            this._func = func;
        }

        public virtual TRes DoTranspose(TArg value)
        {
            return Func(value);
        }

        protected abstract TArg ConvertArgBeforeTransform(object value);

        public virtual object DoTranspose(object value)
        {
            return DoTranspose(ConvertArgBeforeTransform(value));
        }

        protected DefTransposeInit<TArg, TRes> Override(Func<TArg, TRes> handler)
        {
            _func = handler;
            return this;
        }

        public DefTransposeInit<TArg, TRes> For(string propName)
        {
            TargetProperty = propName;
            return this;
        }

        public DefTransposeInit<TArg, TRes> For(Func<string> accessorPropName)
        {
            _targetPropertyAccessor = accessorPropName;
            return this;
        }

        internal virtual DefTransposeInit<TArg, TRes> CopyWith(Func<TArg, TRes> otherEvaluator)
        {
            var copy = (DefTransposeInit<TArg, TRes>)Activator.CreateInstance(this.GetType(), otherEvaluator ?? _func);
            copy._targetPropertyAccessor = _targetPropertyAccessor;
            return copy;
        }
    }

}

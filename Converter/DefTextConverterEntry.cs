using JLVidalTranspose.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JLVidalTranspose.Core
{
    public abstract class DefTextConverterEntry<T> : DefTextConverterBase<T>
    {
        readonly Lazy<DefPosTransposition[]> _posTransforms;
        readonly Lazy<DefTextPreTypeTransposer[]> _posParsers;
        readonly Lazy<PropertyInfo[]> _modelProperties;
        readonly Lazy<DefTextPreTypeTransposer[]> _preParsers;
        readonly Lazy<IDefTranspose[]> _initTransforms;
        readonly Lazy<IDefTranspose[]> _preTransforms;

        public DefTextConverterEntry()
        {
            _initTransforms = new Lazy<IDefTranspose[]>(BuildInitTransforms);
            _preParsers = new Lazy<DefTextPreTypeTransposer[]>(BuildPreParsers);
            _preTransforms = new Lazy<IDefTranspose[]>(BuildPreTransforms);
            _posTransforms = new Lazy<DefPosTransposition[]>(BuildPosTransforms);
            _posParsers = new Lazy<DefTextPreTypeTransposer[]>(BuildPosParsers);
            _modelProperties = new Lazy<PropertyInfo[]>(BuildModelProperties);
        }

        protected abstract IEnumerable<IDefTranspose> InitTreatmentTransforms();
        protected abstract IEnumerable<DefTextPreTypeTransposer> PreParsers();
        protected abstract IEnumerable<DefTransposeInit<object, string>> IntermediateTransforms();
        protected abstract IEnumerable<DefTextPosTransposer> PosParsers();
        protected abstract IEnumerable<DefPosTransposition> PosTransforms();

        public override string Parse(T model)
        {
            var fields = GetDefiniedFields();

            var allProperties = _modelProperties.Value;
            var initTransforms = _initTransforms.Value;
            var preParsers = _preParsers.Value;
            var preTransforms = _preTransforms.Value;
            var posParsers = _posParsers.Value;
            var posTransforms = _posTransforms.Value;

            StringBuilder builder = new StringBuilder();
            bool started = false;
            foreach (var item in fields)
            {
                if (started)
                    builder.Append(Delimiter);

                started = true;

                var prop = allProperties.BinarySearchFirst(b => b.Name, item, StringComparer.OrdinalIgnoreCase) ??
                                                            allProperties.BinarySearchFirst(b => b.Name.Replace("_", " "), item.Replace("_", ""), StringComparer.OrdinalIgnoreCase);
                if (prop == null)
                    throw new NullReferenceException(string.Format("prop='{0}' not found", item));

                var value = prop.GetValue(model, null);

                if (value == null)
                    continue;

                value = ApplyBehaviors(initTransforms, preParsers, preTransforms, posParsers, posTransforms, prop.Name, value);
                builder.Append(value);
            }
            return builder.ToString();
        }

        private static System.Reflection.PropertyInfo[] BuildModelProperties()
        {
            var allProperties = typeof(T).GetProperties().OrderBy(a => a.Name, StringComparer.OrdinalIgnoreCase).ToArray();
            return allProperties;
        }

        private DefPosTransposition[] BuildPosTransforms()
        {
            var posTransforms = PosTransforms().OrderBy(a => a.TargetProperty, StringComparer.OrdinalIgnoreCase).ToArray();
            return posTransforms;
        }

        private DefTextPosTransposer[] BuildPosParsers()
        {
            var posParsers = PosParsers().ToArray();
            return posParsers;
        }

        private IDefTranspose[] BuildPreTransforms()
        {
            var preTransforms = IntermediateTransforms().OrderBy(a => a.TargetProperty, StringComparer.OrdinalIgnoreCase).ToArray();
            return preTransforms;
        }

        private DefTextPreTypeTransposer[] BuildPreParsers()
        {
            var preParsers = PreParsers().OrderBy(a => a.TargetType.FullName, StringComparer.OrdinalIgnoreCase).ToArray();
            return preParsers;
        }

        private IDefTranspose[] BuildInitTransforms()
        {
            var initTransforms = InitTreatmentTransforms().OrderBy(a => a.TargetProperty, StringComparer.OrdinalIgnoreCase).ToArray();
            return initTransforms;
        }

        private object ApplyBehaviors(IDefTranspose[] initTransform, DefTextPreTypeTransposer[] preParsers, IDefTranspose[] preTransforms, DefTextPreTypeTransposer[] genericPosParsers, DefPosTransposition[] posTransforms, string propertyName, object value)
        {
            var toInitTransform = initTransform.BinarySearchMany(a => a.TargetProperty, propertyName, StringComparer.OrdinalIgnoreCase);
            foreach (var transform in toInitTransform)
            {
                value = transform.DoTranspose(value);
            }

            var toPreParse = preParsers.BinarySearchMany(a => a.TargetType.FullName, value.GetType().FullName);
            foreach (var parser in toPreParse)
            {
                value = parser.DoTranspose(value);
            }

            var toPreTransform = preTransforms.BinarySearchMany(b => b.TargetProperty, propertyName, StringComparer.OrdinalIgnoreCase);
            foreach (var transform in toPreTransform)
            {
                value = transform.DoTranspose(value);
            }

            if (!(value is string))
            {
                value = value.ConvertOrDefault<string>();
            }

            foreach (var parser in genericPosParsers)
            {
                value = parser.DoTranspose(value);
            }

            var toPosTransform = posTransforms.BinarySearchMany(b => b.TargetProperty, propertyName, StringComparer.OrdinalIgnoreCase);
            foreach (var transform in toPosTransform)
            {
                value = transform.DoTranspose((string)value);
            }

            return value;
        }
    }
}

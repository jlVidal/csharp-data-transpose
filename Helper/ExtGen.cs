using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JLVidalTranspose.Helper
{
    public static class ExtGen
    {
        public static T GetEnumValue<T>(this string description) where T : struct
        {
            FieldInfo[] fis = typeof(T).GetFields();

            foreach (var fi in fis)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0 && attributes[0].Description.Equals(description, StringComparison.InvariantCultureIgnoreCase))
                    return (T)Enum.Parse(typeof(T), fi.Name);
            }

            return (T)Enum.Parse(typeof(T), description);
        }

        public static object GetValueOrNull<T>(this T? model) where T : struct
        {
            if (!model.HasValue)
                return null;

            return model.Value;

        }

        public static bool GetEnumValue<T>(this string description, out T realValue) where T : struct
        {
            FieldInfo[] fis = typeof(T).GetFields();

            foreach (var fi in fis)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0 &&
                    attributes.Any(obj => obj.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase)))
                {
                    try
                    {
                        realValue = (T)Enum.Parse(typeof(T), fi.Name);
                        return true;
                    }
                    catch
                    {

                    }

                }
            }

            try
            {
                realValue = (T)Enum.Parse(typeof(T), description);
                return true;
            }
            catch
            {
                realValue = default(T);
                return false;
            }

        }

        public static bool EqualsEx(this string text, string comparsion)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(text, comparsion);
        }

        public static string GetStringValue<T>(this T enumValue) where T : struct
        {
            Type type = enumValue.GetType();

            MemberInfo[] memInfo = type.GetMember(enumValue.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return enumValue.ToString();
        }

        public static bool ContainsEx(this string str, string toSearch,
                                        StringComparison comparer = StringComparison.OrdinalIgnoreCase)
        {

            if (str == null)
            {
                if (toSearch == null)
                    return true;

                return false;
            }

            if (toSearch == null)
                throw new NullReferenceException("toSearch");

            return str.IndexOf(toSearch, comparer) > -1;
        }

        public static string ReplaceEx(this string str, string oldValue, string newValue, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            if (oldValue == null)
                throw new ArgumentNullException("oldValue");

            newValue = newValue ?? string.Empty;

            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        public static string ToStringOrEmpty<T>(this T item) where T : class
        {
            if (item == null)
                return string.Empty;

            return item.ToString();
        }

        internal static string DumpException(this Exception ex)
        {
            if (ex == null)
                return String.Format("Exception is null.");

            var allMsg = new StringBuilder();
            int count = 0;
            Exception genericException = ex;

            var avoidsStackOverflow = new List<Exception>();
            try
            {
                do
                {
                    var partialMsg = new StringBuilder();
                    count++;
                    partialMsg.Append("[");
                    partialMsg.Append(count);
                    partialMsg.Append("]");

                    if (count > 1)
                        partialMsg.Append(" InnerException Type: ");
                    else
                        partialMsg.Append(" Exception Type: ");

                    partialMsg.Append(genericException.GetType());

                    if (!string.IsNullOrEmpty(genericException.Message))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("Message: ");
                        partialMsg.Append(genericException.Message);
                    }

                    if (!string.IsNullOrEmpty(genericException.Source))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("Source: ");
                        partialMsg.Append(genericException.Source);
                    }

                    if (!string.IsNullOrEmpty(genericException.StackTrace))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("StackTrace: ");
                        partialMsg.Append(genericException.StackTrace);
                    }


                    if (genericException.TargetSite != null)
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("TargetSite: name=");
                        partialMsg.Append(genericException.TargetSite.Name);

                        partialMsg.Append(", type=");
                        partialMsg.Append(genericException.TargetSite.ReflectedType);

                        partialMsg.Append(", assembly=");
                        partialMsg.Append(genericException.TargetSite.ReflectedType.Assembly);
                    }

                    if (!string.IsNullOrEmpty(genericException.HelpLink))
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("HelpLink: ");
                        partialMsg.Append(genericException.HelpLink);
                    }

                    if (genericException.Data != null && genericException.Data.Count > 0)
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.Append("Data [");
                        partialMsg.Append(genericException.Data.Count);
                        partialMsg.Append("]: ");
                        partialMsg.AppendLine();

                        foreach (DictionaryEntry item in genericException.Data)
                        {
                            try
                            {
                                partialMsg.AppendFormat("Key: {0} | Value: {1}", item.Key, item.Value.ToStringOrEmpty());
                                partialMsg.AppendLine();
                            }
                            catch
                            {

                            }
                        }
                    }

                    Type exceptionType = genericException.GetType();
                    while (exceptionType != typeof(Exception) && exceptionType != null && (exceptionType.IsValueType || exceptionType.IsPrimitive))
                    {
                        var otherProperties = exceptionType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        if (otherProperties.Length > 0)
                        {

                            bool writeHeader = false;
                            foreach (var item in otherProperties)
                            {
                                if (!item.CanRead)
                                    continue;

                                var getMethod = item.GetGetMethod(false);
                                if (getMethod == null || getMethod.GetBaseDefinition() != getMethod)
                                    continue;

                                if (!writeHeader)
                                {
                                    partialMsg.AppendLine();
                                    partialMsg.AppendLine();
                                    partialMsg.Append("Other Information:");
                                    partialMsg.AppendLine();
                                    writeHeader = true;
                                }

                                partialMsg.AppendFormat("   PropertyName: {0}", item.Name).AppendLine();
                                partialMsg.AppendFormat("   Value: {0}", item.GetValue(genericException, null).ToStringOrEmpty());
                                partialMsg.AppendLine();
                            }
                        }

                        exceptionType = exceptionType.BaseType;
                    }

                    if (count != 1)
                    {
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                        partialMsg.AppendLine();
                    }
                    allMsg.Insert(0, partialMsg.ToString());

                    if (avoidsStackOverflow.Any(obj => obj == genericException))
                        break;

                    avoidsStackOverflow.Add(genericException);
                    genericException = genericException.InnerException;


                } while (genericException != null);
            }
            catch
            {

            }

            return allMsg.ToString();
        }

        public static string GetMemName<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var body = expression.Body as MemberExpression;
            if (body != null)
            {
                var memberExpression = body;
                return memberExpression.Member.Name;
            }
            var unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                var memberExp = unaryExpression.Operand as MemberExpression;
                if (memberExp != null)
                {
                    return memberExp.Member.Name;
                }
            }
            throw new Exception(string.Format("Expression type ({0})", expression.GetType()));
        }

        public static string GetLastNodeName<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var body = expression.Body as MemberExpression;
            if (body != null)
            {
                var memberExpression = body;
                return memberExpression.Member.Name;
            }
            var unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                var memberExp = unaryExpression.Operand as MemberExpression;
                if (memberExp != null)
                    return memberExp.Member.Name;
            }
            throw new Exception(string.Format("Expression type ({0})", expression.GetType()));
        }

        public static T ConvertOrDefault<T>(this object objectIn)
        {
            if (objectIn is T)
                return (T)objectIn;

            if (objectIn == null || Convert.IsDBNull(objectIn))
            {
                return default(T);
            }
            try
            {
                var res = (T)Convert.ChangeType(objectIn, typeof(T));
                return res;
            }
            catch
            {
                return default(T);
            }
        }

        public static T ConvertOrDefaultOrEmpty<T>(this object objectIn)
        {
            if (objectIn is T)
                return (T)objectIn;

            if (objectIn == null || Convert.IsDBNull(objectIn))
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)((object)string.Empty);
                }
                return default(T);
            }
            try
            {
                var res = (T)Convert.ChangeType(objectIn, typeof(T));
                if (res == null)
                {
                    if (typeof(T) == typeof(string))
                    {
                        return (T)((object)string.Empty);
                    }
                }
                return res;
            }
            catch
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)((object)string.Empty);
                }
                return default(T);
            }
        }

        public static bool ConvertTo<T>(this object objectIn, out T value)
        {
            if (objectIn is T)
            {
                value = (T)objectIn;
                return true;
            }

            value = default(T);
            if (objectIn == null || Convert.IsDBNull(objectIn))
                return false;

            try
            {
                value = (T)Convert.ChangeType(objectIn, typeof(T));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static TResult Return<TInput, TResult>(this TInput o,
       Func<TInput, TResult> evaluator, TResult failureValue = default(TResult)) where TInput : class
        {
            if (o == null) 
                return failureValue;
            return evaluator(o);
        }

        public static TResult With<TInput, TResult>(this TInput o,
       Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o);
        }

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
       where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
       where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }
    }
}

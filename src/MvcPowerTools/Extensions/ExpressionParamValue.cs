using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MvcPowerTools.Extensions
{
    class ExpressionParamValue
    {
        public static object ForExpression(Expression data)
        {
            switch(data.NodeType)
            {
                case ExpressionType.Constant:
                    return FromConstant(data as ConstantExpression);
                case ExpressionType.MemberAccess:
                    return FromMember(data as MemberExpression);
            }
            throw new InvalidOperationException("Can't get the value of expression, it must be a constant, field or a property");
        }

        static object FromConstant(ConstantExpression data)
        {
            return data.Value;
        }

        static object FromMember(MemberExpression data)
        {
            var o = new ExpressionObjectValue(data);
            return GetMemberValue(o.GetValue(), data.Member);
        }

        public static object GetMemberValue(object obj, MemberInfo f)
        {
            switch (f.MemberType)
            {
                case MemberTypes.Field: return GetFieldValue(f as FieldInfo, obj);
                case MemberTypes.Property:
                    return GetPropertyValue(f as PropertyInfo, obj);
            }
            throw new NotSupportedException();
        }

        private static object GetFieldValue(FieldInfo f, object obj)
        {
            return f.GetValue(obj);
        }

        private static object GetPropertyValue(PropertyInfo p, object obj)
        {
            return p.GetValue(obj, null);
        }
    }
}
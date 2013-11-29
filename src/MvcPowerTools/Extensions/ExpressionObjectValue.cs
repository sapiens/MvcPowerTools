using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MvcPowerTools.Extensions
{
    class ExpressionObjectValue
    {
        private MemberExpression _data;

        public ExpressionObjectValue(MemberExpression data)
        {
            _data = data;
            
        }

       
        public object GetValue()
        {
            EstablishRoot();
            object rez = null;
            var crnt = _stack.Pop();
            var c = crnt.Expression as ConstantExpression;
            rez = c.Value;
            
            while(_stack.Count>0)
            {
                rez = ExpressionParamValue.GetMemberValue(rez, crnt.Member);
                crnt = _stack.Pop();              
            }
            return rez;
        }

        private Stack<MemberExpression> _stack=new Stack<MemberExpression>();

        void EstablishRoot()
        {
            var r = _data;
            while(true)
            {
                _stack.Push(r);
                if (r.Expression.NodeType==ExpressionType.Constant)
                {
                    break;
                }
                if (r.Expression.NodeType==ExpressionType.MemberAccess)
                {
                    r = _data.Expression as MemberExpression;                    
                }
                else
                {
                    throw new Exception("not supported");
                }
            }
        }
    }
}
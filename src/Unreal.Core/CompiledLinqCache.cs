using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Unreal.Core
{
    public class CompiledLinqCache
    {
        private Dictionary<Type, Func<object>> _compiledBuilders = new Dictionary<Type, Func<object>>();

        public object CreateObject(Type type)
        {
            if(_compiledBuilders.TryGetValue(type, out Func<object> builder))
            {
                return builder();
            }

            BlockExpression block = Expression.Block(type, Expression.New(type));
            builder = Expression.Lambda<Func<object>>(block).Compile();

            _compiledBuilders[type] = builder;

            return builder();
        }
    }
}

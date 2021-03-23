using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Unreal.Core
{
    public class CompiledLinqCache
    {
        private Dictionary<Type, Func<dynamic>> _compiledBuilders = new Dictionary<Type, Func<dynamic>>();

        public object CreateObject(Type type)
        {
            if(_compiledBuilders.TryGetValue(type, out Func<dynamic> builder))
            {
                return builder();
            }

            BlockExpression block = Expression.Block(type, Expression.New(type));
            builder = Expression.Lambda<Func<dynamic>>(block).Compile();

            _compiledBuilders[type] = builder;

            return builder();
        }
    }
}

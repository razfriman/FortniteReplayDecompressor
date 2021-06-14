using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using Unreal.Core.Contracts;
using Unreal.Core.Extensions;

namespace Unreal.Core
{
    public sealed class CompiledLinqCache
    {
        private Dictionary<Type, Func<IProperty>> _compiledIPropertyBuilders = new Dictionary<Type, Func<IProperty>>();

        private KeyList<Type, Func<INetFieldExportGroup>> _compiledBuilders = new KeyList<Type, Func<INetFieldExportGroup>>();

        internal int AddExportType(Type type)
        {
            if(_compiledBuilders.TryGetIndex(type, out int index))
            {
                return index;
            }

            _compiledBuilders.Add(type, CreateFunction<INetFieldExportGroup>(type));

            return _compiledBuilders.Count - 1;
        }

        public INetFieldExportGroup CreateObject(int typeId)
        {
            return _compiledBuilders[typeId]();
        }


        public IProperty CreatePropertyObject(Type type)
        {
            if (_compiledIPropertyBuilders.TryGetValue(type, out Func<IProperty> builder))
            {
                return builder();
            }

            builder = CreateFunction<IProperty>(type);
            _compiledIPropertyBuilders[type] = builder;

            return builder();
        }

        private Func<T> CreateFunction<T>(Type type)
        {
            BlockExpression block = Expression.Block(type, Expression.New(type));
            Func<T> builder = Expression.Lambda<Func<T>>(block).Compile();

            return builder;
        }
    }
}

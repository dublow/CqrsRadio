using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CqrsRadio.Common.AssemblyScanner
{
    public static class TypeScanner
    {
        public static List<Type> GetTypesOf<T>()
        {
            var assemblies = GetLocalAssemblies();
            var manyTypes = assemblies
                .SelectMany(x => x.GetTypes());

            return manyTypes
                .Where(x => typeof(T).IsAssignableFrom(x)
                            && x.IsClass)
                .Where(x => x != typeof(T))
                .ToList();
        }

        static IEnumerable<Assembly> GetLocalAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(x =>
                    !x.IsDynamic).ToList();
        }
    }
}

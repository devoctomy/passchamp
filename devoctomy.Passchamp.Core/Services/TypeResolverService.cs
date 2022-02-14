using System;
using System.Reflection;

namespace devoctomy.Passchamp.Core.Services
{
    public class TypeResolverService : ITypeResolverService
    {
        public Type GetType(string name)
        {
            if(name.Contains(':'))
            {
                var typeParts = name.Split(':');
                var assembly = Assembly.LoadFrom($"{typeParts[0]}.dll");
                return assembly.GetType(typeParts[1]);
            }

            return Type.GetType(name);
        }
    }
}

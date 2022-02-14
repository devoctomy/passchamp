using System;

namespace devoctomy.Passchamp.Core.Services
{
    public interface ITypeResolverService
    {
        public Type GetType(string name);
    }
}

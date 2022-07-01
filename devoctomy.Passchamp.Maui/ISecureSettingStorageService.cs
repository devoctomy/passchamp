using System;
using System.Reflection;

namespace devoctomy.Passchamp.Maui
{
    public interface ISecureSettingStorageService
    {
        public Task<bool> LoadAsync(
            string id,
            PropertyInfo property,
            object instance);
        public Task SaveAsync(
            string id,
            PropertyInfo property,
            object instance);
    }
}

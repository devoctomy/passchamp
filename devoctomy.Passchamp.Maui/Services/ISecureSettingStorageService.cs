using System;
using System.Reflection;

namespace devoctomy.Passchamp.Maui.Services
{
    public interface ISecureSettingStorageService
    {
        public bool IsApplicable(PropertyInfo property);
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

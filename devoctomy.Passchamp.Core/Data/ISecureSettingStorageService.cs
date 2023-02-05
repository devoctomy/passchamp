using System;
using System.Reflection;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data;

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
    public bool Remove(
        string id,
        PropertyInfo property);
    public void RemoveAll();
}

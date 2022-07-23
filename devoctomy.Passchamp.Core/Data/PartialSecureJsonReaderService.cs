﻿using devoctomy.Passchamp.Core.Exceptions;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace devoctomy.Passchamp.Core.Data
{
    public class PartialSecureJsonReaderService : IPartialSecureJsonReaderService
    {
        private ISecureSettingStorageService _secureSettingStorageService;

        public PartialSecureJsonReaderService(ISecureSettingStorageService secureSettingStorageService)
        {
            _secureSettingStorageService = secureSettingStorageService;
        }

        public async Task<T> LoadAsync<T>(Stream stream)
        {
            JsonSerializer serializer = new JsonSerializer();
            T result;
            using (StreamReader sr = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                result = serializer.Deserialize<T>(reader);
            }

            await LoadSecureSettingsAsync(result);
            return result;
        }

        private async Task LoadSecureSettingsAsync<T>(T value)
        {
            var partiallySecure = value as IPartiallySecure;
            var type = value.GetType();
            var allProperties = type.GetProperties(
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance).ToList();
            foreach (var curProperty in allProperties)
            {
                if (_secureSettingStorageService.IsApplicable(curProperty))
                {
                    if (partiallySecure == null)
                    {
                        throw new ObjectDoesNotImplementIPartiallySecureException(type);
                    }

                    await _secureSettingStorageService.LoadAsync(
                        partiallySecure.Id,
                        curProperty,
                        value);
                }
            }
        }
    }
}
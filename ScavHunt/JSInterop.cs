﻿using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace ScavHunt
{
    public class JSInterop
    {
        IJSRuntime js;

        public JSInterop(IJSRuntime js)
        {
            this.js = js;
        }

        public async Task SetStorage(string key, string value)
        {
            await js.InvokeVoidAsync("SetLocalStorage", key, value);
        }

        public async Task SetStorageObject<T>(string key, T value)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await SetStorage(key, json);
        }

        public async Task<string> GetStorage(string key)
        {
            return await js.InvokeAsync<string>("GetLocalStorage", key);
        }

        public async Task<T?> GetStorageObject<T>(string key)
        {
            try
            {
                var json = await GetStorage(key);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Get Storage Object Exception: {e.Message}");
            }

            return default;
        }

        public async Task ClearStorage()
        {
            await js.InvokeVoidAsync("ClearLocalStorage");
        }
    }
}

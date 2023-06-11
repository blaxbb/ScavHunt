using Microsoft.JSInterop;
using Newtonsoft.Json;
using ScavHunt.Components;

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

        public async Task ShowModal(string id)
        {
            await js.InvokeVoidAsync("ShowModal", id);
        }

        public async Task HideModal(string id)
        {
            await js.InvokeVoidAsync("HideModal", id);
        }

        public async Task SetupScanner(DotNetObjectReference<ScannerModal> objRef, string modal, string id)
        {
            await js.InvokeVoidAsync("SetupScanner", objRef, modal, id);
        }

        public async Task SelectValue(string id, string value)
        {
            await js.InvokeVoidAsync("SelectValue", id, value);
        }

        public async Task InitSortable(string id)
        {
            await js.InvokeVoidAsync("InitSortable", id);
        }

        public record QuestionTree(string id, List<QuestionTree> children);

        public async Task<List<QuestionTree>> GetQuestionTree(string id)
        {
            return await js.InvokeAsync<List<QuestionTree>>("GetQuestionTree", id);
        }

        public async Task StartDownload(string filename, Stream stream)
        {
            using var streamRef = new DotNetStreamReference(stream);
            await js.InvokeVoidAsync("downloadFileFromStream", filename, streamRef);
        }
        public async Task HideNav(string id)
        {
            try
            {
                await js.InvokeVoidAsync("closeNav", id);
            }
            catch(Exception e)
            {

            }
        }

        public async Task CreateQrCode(string id, string text)
        {
            await js.InvokeVoidAsync("CreateQrCode", id, text);
        }
    }
}

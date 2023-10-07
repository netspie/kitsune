using Blazored.LocalStorage;
using Corelibs.Basic.Collections;

namespace Manabu.UI.Common.Storage;

public class BlazoredJsonLocalStorage : IStorage
{
    private readonly ILocalStorageService _localStorage;

    public BlazoredJsonLocalStorage(
        ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task Save<T>(T @object)
    {
        var key = typeof(T).Name;
        await _localStorage.SetItemAsync(key, @object);
    }

    public async Task Save(object @object, Type type)
    {
        if (@object is null)
            return;

        var key = type.Name;
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(@object);
        await _localStorage.SetItemAsStringAsync(key, json);
    }

    public async Task<T> Get<T>()
    {
        var key = typeof(T).Name;
        return await _localStorage.GetItemAsync<T>(key);
    }

    public async Task<object> Get(Type type)
    {
        var key = type.Name;
        var json = await _localStorage.GetItemAsStringAsync(key);
        if (json.IsNullOrEmpty())
            return null;

        return Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
    }

    public async Task Delete<T>()
    {
        var key = typeof(T).Name;
        await _localStorage.RemoveItemAsync(key);
    }
}

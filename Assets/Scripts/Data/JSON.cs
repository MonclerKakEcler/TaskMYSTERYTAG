using Newtonsoft.Json;
using Cysharp.Threading.Tasks;
using System.IO;

public static class JSON
{
    public static string ToJSON<T>(T data)
    {
        return JsonConvert.SerializeObject(data);
    }

    public static T FromJSON<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    public async static UniTask<string> ToJSONAsync<T>(T data)
    {
        return await UniTask.RunOnThreadPool(() => ToJSON(data));
    }

    public async static UniTask<T> FromJSONAsync<T>(string data)
    {
        return await UniTask.RunOnThreadPool<T>(() => FromJSON<T>(data));
    }

    public async static UniTask JsonSave<T>(T data, string dirPath, string fileName)
    {
        var json = await ToJSONAsync(data);
        var path = $"{dirPath}/{fileName}.json";
        await File.WriteAllTextAsync(path, json);
    }

    public async static UniTask<T> JsonRead<T>(string dirPath, string fileName) where T : new()
    {
        T data;
        var path = $"{dirPath}/{fileName}.json";

        if (!File.Exists(path))
        {
            data = new T();
            await JsonSave(data, dirPath, fileName);
            return data;
        }

        var json = await File.ReadAllTextAsync(path);
        data = await FromJSONAsync<T>(json);

        if (data == null)
        {
            data = new T();
            await JsonSave(data, dirPath, fileName);
            return data;
        }
        return data;
    }
}
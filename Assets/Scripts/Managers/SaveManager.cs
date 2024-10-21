using Newtonsoft.Json;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    private readonly string SAVE_DIR = "SaveDatas";

    public void Save<T>(T data) where T : class
    {
        string json = JsonConvert.SerializeObject(data);

        if (Directory.Exists(SAVE_DIR) == false)
            Directory.CreateDirectory(SAVE_DIR);

        string path = Path.Combine(SAVE_DIR, typeof(T).Name + ".json");
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        File.WriteAllBytes(path, bytes);
    }

    public T Load<T>() where T : class, new()
    {
        string path = Path.Combine(SAVE_DIR, typeof(T).Name + ".json");
        if (File.Exists(path) == false) return new T();
        byte[] bytes = File.ReadAllBytes(path);
        string json = System.Text.Encoding.UTF8.GetString(bytes);
        return JsonConvert.DeserializeObject<T>(json);
    }
}

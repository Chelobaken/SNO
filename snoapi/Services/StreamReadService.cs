
using System.Text;
using System.Text.Json;


#nullable disable

namespace SNO.API;

public class JsonFileReadService
{
    private StreamReader reader;
    private FileStream stream;
    private IConfiguration appConfig;
    private JsonDocument jsonData;

    public JsonFileReadService(IConfiguration configuration)
    {
        appConfig = configuration;
        stream = new FileStream(appConfig["NonRelationalData:SkillsTreeFile"], FileMode.Open, FileAccess.Read);
        reader = new StreamReader(stream);
    }

    public async Task<JsonDocument> ReadJsonAsync()
    {
        byte[] bytes = Encoding.UTF8.GetBytes(reader.ReadToEnd());
        jsonData = await JsonDocument.ParseAsync(stream);

        return jsonData;
    }

    public JsonDocument ReadJson()
    {
        jsonData = JsonDocument.Parse(stream);
        return jsonData;
    }
}
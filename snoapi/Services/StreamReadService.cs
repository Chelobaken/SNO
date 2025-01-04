
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


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

public class ImageFileReadService
{

    public enum FileReadResult
    {
        Ok = 0,
        FileNotFound = -1,
        InsecureRead = 1,
    }
    
    private IConfiguration appConfig;
    private FileStream fileStream;
    private const string INSECURE_PATH_REGEX = @"((\.\.)+/?)+"; 

    public ImageFileReadService(IConfiguration configuration)
    {
        appConfig = configuration;
    }

    public FileStream ReadImage(string FileName, out FileReadResult result)
    {
        string path = appConfig["NonRelationalData:ImagesDir"] + "/" + FileName;
        
        if(Regex.IsMatch(FileName, INSECURE_PATH_REGEX))
        {
            result = FileReadResult.InsecureRead;
            return null;
        }

        
        if(!File.Exists(path))
        {
            result = FileReadResult.FileNotFound;
            return null;
        }
        
        fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);


        result = FileReadResult.Ok;
        return fileStream;
    }
}
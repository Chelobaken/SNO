using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace SNO.API;

[ApiController]
[Route("/api/skills")]
public class SkillsController : ApiController<JsonObject>
{

    private JsonFileReadService jsonReader;
    
    public SkillsController(JsonFileReadService jsonReader, ILogger<ApiController<JsonObject>> logger, SnoDB dbCtx) 
    : base(logger, dbCtx, null)
    {
        this.jsonReader = jsonReader;
    }
    
    
    [HttpGet]
    [Route("")]
    public JsonResult GetSkillsTree()
    {
        JsonDocument json = jsonReader.ReadJson();
        return new JsonResult(json.RootElement);
    }
    
    // [HttpGet]
    // [Route("{institutionName}/{departmentName}")]
    // public JsonResult GetSkillsByDepartment(string institutionName, string departmentName)
    // {
    //      JsonDocument json = jsonReader.ReadJson();

    //     json.RootElement.GetProperty("institutions").EnumerateArray()
    // }
    
    public override JsonResult GetSchema()
    {
        throw new NotImplementedException();
    }
}
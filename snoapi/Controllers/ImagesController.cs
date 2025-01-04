using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace SNO.API;

[ApiController]
[Route("/api/images")]
public class ImageController : ApiController<FileStream>
{

    private ImageFileReadService imageReader;
    
    public ImageController(ImageFileReadService fileReadService, ILogger<ImageController> logger, SnoDB dbCtx) 
    : base(logger, dbCtx, null)
    {
        this.imageReader = fileReadService;
    }
    
    
    [HttpGet]
    [Route("{name}")]
    public ActionResult Get(string name)
    {
        ImageFileReadService.FileReadResult r;
        FileStream stream = imageReader.ReadImage(name, out r);

        switch(r)
        {
            case ImageFileReadService.FileReadResult.FileNotFound:
            {
                return new NotFoundResult();
            }
            case ImageFileReadService.FileReadResult.InsecureRead:
            {
                return new BadRequestResult();
            }
            default:
                return new FileStreamResult(stream, "image/jpeg");
        }
        
        
    }
    
    
    
    public override JsonResult GetSchema()
    {
        throw new NotImplementedException();
    }
}
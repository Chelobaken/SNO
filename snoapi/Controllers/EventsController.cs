using Microsoft.AspNetCore.Mvc;


namespace SNO.API;

[ApiController]
[Route("/api/events")]
public class EventsController : ApiController<Event>
{
    public EventsController(ILogger<EventsController> logger, SnoDB dbContext, SnoWriterService<Event> snoWriter)
    : base(logger, dbContext, snoWriter)
    {

    }

    [Route("{id?}")]
    [HttpGet]

    public IActionResult Get(int? id)
    {
        if (id != null)
            return new JsonResult(_dbContext.Events.Where(e => e.Eventid == id));
        else
            return new JsonResult(_dbContext.Events.ToList());
    }

    [Route("new")]
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        //return new JsonResult("hhh");

        if (Request.ContentLength == 0)
            return BadRequest();

        await snoWriter.WriteData(_dbContext.Events, Request);
        _dbContext.SaveChanges();

        return Ok();
    }

    [Route("schema")]
    [HttpGet]
    public override JsonResult GetSchema()
    {
        return new JsonResult("nil");
    }
}
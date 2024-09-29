using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace snoapi.Controllers;


#nullable disable


public class Selector<T> where T : class
{
    protected DbSet<T> set;
    
    public Selector(DbSet<T> set)
    {
        this.set = set;
    }

    public virtual IEnumerable<T> Select()
    {
        return set.ToList();
    }
}

public class EventByIdSelector : Selector<Event>
{
    private int? id; 
    
    public EventByIdSelector(int? id, DbSet<Event> set) : base(set)
    {
        this.set = set;
        this.id = id;
    }

    public override List<Event> Select() => set.Where(e => e.Eventid == id).ToList();
}

public class ProjectByIdSelector : Selector<Project>
{
    private int? id; 
    
    public ProjectByIdSelector(int? id, DbSet<Project> set) : base(set)
    {
        this.set = set;
        this.id = id;
    }

    public override List<Project> Select() => set.Where(e => e.Projectid == id).ToList();
}

public class SnoWriterService<T> where T : class
{
    public SnoWriterService()
    {

    }
    
    public virtual async Task<bool> WriteData(DbSet<T> dataSet, HttpRequest request)
    {
        int streamLength = (int)request.ContentLength;

        byte[] bytes = new byte[streamLength];
        await request.Body.ReadAsync(bytes);

        string json = Encoding.UTF8.GetString(bytes);

        T e = JsonConvert.DeserializeObject<T>(json);

        await dataSet.AddAsync(e);

        return true;
    }
}

public abstract class ApiController<T> : ControllerBase where T : class
{
    protected readonly ILogger<ApiController<T>> _logger;
    protected readonly SnoDB _dbContext;
    protected readonly SnoWriterService<T> snoWriter;
    
    public ApiController(ILogger<ApiController<T>> logger, SnoDB db, SnoWriterService<T> snoWriter)
    {
        _logger = logger;
        _dbContext = db;
        this.snoWriter = snoWriter;
    }

    public abstract JsonResult GetSchema();
}



[ApiController]
[Route("/api/events")]
public class EventsController : ApiController<Event>
{
    public EventsController(ILogger<EventsController > logger, SnoDB dbContext, SnoWriterService<Event> snoWriter)
    : base (logger, dbContext, snoWriter)
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
        
        if(Request.ContentLength == 0)
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


[ApiController]
[Route("/api/projects")]

public class ProjectController : ApiController<Project>
{
    public ProjectController(ILogger<ProjectController > logger, SnoDB dbContext, SnoWriterService<Project> snoWriter)
    : base (logger, dbContext, snoWriter)
    {

    }

    
    [Route("{id?}")]
    [HttpGet]
    public IActionResult Get(int? id)
    {
        if (id != null)
            return new JsonResult(_dbContext.Projects.Where(e => e.Projectid == id));
        else
            return new JsonResult(_dbContext.Events.ToList());
    }

    [Route("new")]
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        await snoWriter.WriteData(_dbContext.Projects, Request);
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


[ApiController]
[Route("/api/users")]
public class UsersController : ApiController<User>
{
    public UsersController(ILogger<UsersController > logger, SnoDB dbContext, SnoWriterService<User> snoWriter) : 
     base(logger, dbContext, snoWriter)
    {
        
    }

    [Route("{id?}")]
    [HttpGet]
    public IActionResult Get(int? id)
    {
        if (id != null)
            return new JsonResult(_dbContext.Users.Where(e=>e.Userid == id));
        else
            return new JsonResult(_dbContext.Events.ToList());
    }

    [Route("new")]
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        await snoWriter.WriteData(_dbContext.Users, Request);
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
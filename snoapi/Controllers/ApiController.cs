using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace snoapi.Controllers;



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

public class SnoWriterService<T> where T : class
{
    public SnoWriterService()
    {

    }
    
    public virtual void WriteData(DbSet<T> dataSet, Stream requestBodyStream)
    {
        long streamLength = requestBodyStream.Length;

        byte[] bytes = new byte[streamLength];
        requestBodyStream.Read(bytes);

        string json = Encoding.UTF8.GetString(bytes);

        T? e = JsonConvert.DeserializeObject<T>(json);

        dataSet.Add(e);
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
}


public abstract class EventsController : ApiController<Event>
{
    public EventsController(ILogger<EventsController > logger, SnoDB dbContext, SnoWriterService<Event> snoWriter)
    : base (logger, dbContext, snoWriter)
    {

    }

    public IActionResult Get(int? id)
    {
        if (id == null)
            return new JsonResult(new EventByIdSelector(id, _dbContext.Events).Select());
        else
            return new JsonResult(_dbContext.Events.Where(e => e.Eventid == id));
    }

    public IActionResult Post()
    {
        snoWriter.WriteData(_dbContext.Events, Request.Body);
        _dbContext.SaveChanges();

        return Ok();
    }
}


public abstract class UsersController : ApiController<User>
{
    public UsersController(ILogger<UsersController > logger, SnoDB dbContext, SnoWriterService<User> snoWriter) : 
     base(logger, dbContext, snoWriter)
    {
        
    }

    public IActionResult Get(int? id)
    {
        if (id == null)
            return new JsonResult(_dbContext.Users.ToList());
        else
            return new JsonResult(_dbContext.Users.Where(e => e.Userid == id));
    }

    public IActionResult Post()
    {
        Stream bodyStream = this.Request.Body;
        long streamLength = bodyStream.Length;

        byte[] bytes = new byte[streamLength];
        bodyStream.Read(bytes);

        string json = Encoding.UTF8.GetString(bytes);

        User? e = JsonConvert.DeserializeObject<User>(json);

        _dbContext.Users.Add(e);
        _dbContext.SaveChanges();

        return Ok();
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SNO.API;


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
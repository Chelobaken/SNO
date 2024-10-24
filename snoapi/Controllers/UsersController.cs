using Microsoft.AspNetCore.Mvc;



namespace SNO.API;

[ApiController]
[Route("/api/users")]
public class UsersController : ApiController<User>
{
    public UsersController(ILogger<UsersController> logger, SnoDB dbContext, SnoWriterService<User> snoWriter) :
     base(logger, dbContext, snoWriter)
    {

    }

    [Route("{id?}")]
    [HttpGet]
    public IActionResult GetUser(int? id)
    {
        if (id != null)
            return new JsonResult(_dbContext.Users.Where(e => e.Userid == id));
        else
            return new JsonResult(_dbContext.Users.ToList());
    }

    [Route("{id}/projects")]
    [HttpGet]
    public IActionResult GetRelatedProjects(int id)
    {
        var authorsWithProjects = _dbContext.Projectauthors.Join(_dbContext.Projects,
                    author => author.Projectid, project => project.Projectid,
                    (author, project) =>
                    new { ProjectAuthorId = author.Userid, Title = project.Title, Description = project.Mddescription }).ToList();

        var projectsOfSpecifiedUser = authorsWithProjects.Where(entry => entry.ProjectAuthorId == id);

        return new JsonResult(projectsOfSpecifiedUser);
    }

    //[Route("new")]
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
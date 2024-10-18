using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace SNO.API;


[ApiController]
[Route("/api/projects")]

public class ProjectController : ApiController<Project>
{
    public ProjectController(ILogger<ProjectController> logger, SnoDB dbContext, SnoWriterService<Project> snoWriter)
    : base(logger, dbContext, snoWriter)
    {

    }


    [Route("{id?}")]
    [HttpGet]
    public IActionResult Get(int? id)
    {
        if (id != null)
            return new JsonResult(_dbContext.Projects.Where(e => e.Projectid == id));
        else
            return new JsonResult(_dbContext.Projects.ToList());
    }


    [Route("{id}/authors")]
    [HttpGet]
    public IActionResult GetRelatedProjects(int id)
    {
        var projectsWithAuthors = _dbContext.Projectauthors.Join(_dbContext.Users,
                    author => author.Userid, user => user.Userid,
                    (author, user) =>
                    new
                    {
                        ProjectId = author.Projectid,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Patronymic = user.Patronymic,
                        Email = user.Email,
                        VkUsername = user.Usernamevk,
                        TelegramUsername = user.Usernametg,
                        PhoneNumber = user.Phonenumber
                    }).ToList();

        var authorsOfSpecifiedProject = projectsWithAuthors.Where(entry => entry.ProjectId == id);

        return new JsonResult(authorsOfSpecifiedProject);
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
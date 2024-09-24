using System.Collections.Generic;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using ReactDemo.Models;
using SNO;

namespace ReactDemo.Controllers
{
    public enum ApiDataType
    {
        Event,
        Project
    }
    
    public class ApiController : Controller
    {
        private SnoTestDbContext ctx;
        
        public ApiController(SnoTestDbContext context1)
        {
           ctx = context1;
           
           // foreach(IConfigurationSection s in config.GetChildren())
            //    Console.WriteLine(s.Key + " = " + s.Value);
        }

        public ActionResult GetAll( ApiDataType apiData)
        {
            switch(apiData)
            {
                case ApiDataType.Event:
                {
                    List<Event> events = ctx.Events.ToList();
            return Json(events);

                }
                default:
                return Json("null");
            }
            
            
        }
    }
}
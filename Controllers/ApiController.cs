using System.Collections.Generic;
using System.Data.Common;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ReactDemo.Models;
using SNO;

namespace ReactDemo.Controllers
{
    public enum ApiDataType
    {
        Event,
        Project,
        User
    }
    
    public class ApiController : Controller
    {
        private SnoTestDbContext _context;

        public ApiController(SnoTestDbContext context)
        { 
           _context = context;
        }

        public virtual ActionResult Get() 
        {
            
            
            return Json("not implemented");
        }

        public virtual ActionResult Put()
        {
            
        }

        public virtual ActionResult Delete()
        {
            
        }

        public virtual ActionResult Update()
        {
            
        }
    }
}
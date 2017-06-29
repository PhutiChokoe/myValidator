using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyValidatorService.Models;
using Newtonsoft.Json;

namespace MyValidatorService.Controllers
{
    public class IdValidatorController : ApiController
    {
        private readonly IdValidator _validateId = new IdValidator();
        public string Get()
        {
            return "validator api runnning";
        }

      
        public JsonResponse Post([FromBody] dynamic request)
        {
            
            return _validateId.ValidateId(request.id.ToString());
        }


    }
}

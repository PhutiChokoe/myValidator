using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyValidatorService.Database;

namespace MyValidatorService.Models
{
    public class JsonResponse
    {
        public bool Valid { get; set; }
        public UserDetail UserDetails { get; set; }
        public IList<ErrorLists> ErrorLists { get; set; }
    }
}
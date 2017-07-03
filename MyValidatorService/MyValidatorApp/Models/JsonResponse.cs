using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyValidatorService.Models;

namespace MyValidatorApp.Models
{
    public class JsonResponse
    {
        public bool Valid { get; set; }
        public UserDetails UserDetails { get; set; }
        public List<string> ErrorLists { get; set; }
    }
}
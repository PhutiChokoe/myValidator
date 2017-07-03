
using System;
using System.Web.Mvc;
using MyValidatorService.Interfaces;
using MyValidatorService.Models;
using NLog;

namespace MyValidatorService.Controllers
{
    public class IdValidatorController : Controller 
    {
        private IdValidator _validateId = new IdValidator();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private IDbHandler _IDbHandler { get; set; }
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                _IDbHandler = new DbHandler();
                var results = _IDbHandler.GetUserDetails();
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }

        }
        [HttpPost]
        public ActionResult Index(string id)
        {
            try
            {
                var results = _validateId.ValidateId(id);
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }

        }


    }
}

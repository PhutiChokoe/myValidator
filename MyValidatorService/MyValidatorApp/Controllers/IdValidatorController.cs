using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MyValidatorApp.Interfaces;
using MyValidatorApp.Models;
using MyValidatorApp.ViewModel;
using MyValidatorService.Models;
using NLog;

namespace MyValidatorApp.Controllers
{
    [HandleError]
    public class IdValidatorController : Controller
    {
        private IConnectionClient _iConnectionClient { get; set; }
        List<UserDetails> _userDetails = new List<UserDetails>();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        List<String> _errorList = new List<string>();
        public ActionResult Index()
        {
            try
            {
                _iConnectionClient = new ClientConnection();
                dynamic Response = _iConnectionClient.GetSavedUserDetails();
                if (Response == null)
                    ViewBag.result = new List<UserDetails>();
                else
                    ViewBag.result = GetUserDetails(Response);
                _errorList.Clear();
                ViewBag.Error = _errorList;
                return View();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
            finally
            {
                _iConnectionClient.CloseConnection();
            }
        }
        [HttpPost]
        public ActionResult Index(IdValidatorModel id)
        {
            _iConnectionClient = new ClientConnection();
            if (!string.IsNullOrEmpty(id.IdNumber))
            {
                try
                {
                    dynamic Response = _iConnectionClient.GetServiceResponse(id.IdNumber);

                    string valid = Response.Valid;

                    if (valid == "True")
                    {
                        UserDetails result = GetUserDetails(Response);
                        _userDetails.Add(result);
                        ViewBag.Error = _errorList;
                        ViewBag.result = _userDetails;
                    }
                    else
                    {

                        dynamic UserDetail = _iConnectionClient.GetSavedUserDetails();
                        if (UserDetail == null)
                        {
                            ViewBag.result = new List<UserDetails>();
                        }
                        else
                        {
                            if (ViewBag.result.Count == 0)
                            {
                                ViewBag.result = new List<UserDetails>();
                            }
                            else
                            {
                                foreach (var user in UserDetail.UserDetails)
                                {

                                    _userDetails.Add(GetUserDetails(user));
                                }

                                ViewBag.result = _userDetails;
                            }
                        }

                        foreach (var error in Response.ErrorLists)
                        {

                            _errorList.Add(error.error.ToString());
                        }
                        ViewBag.Error = _errorList;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex.Message);
                    throw;
                }
                finally
                {
                    _iConnectionClient.CloseConnection();
                }
            }
            return View();
        }
        public UserDetails GetUserDetails(dynamic Response)
        {
            return new UserDetails
            {
                IdNumber = Response.UserDetails.Id,
                Citizenship = Response.UserDetails.Citizenship,
                DateOfBirth = Response.UserDetails.DateOfBirth,
                Gender = Response.UserDetails.Gender,

            };
        }
    }
}
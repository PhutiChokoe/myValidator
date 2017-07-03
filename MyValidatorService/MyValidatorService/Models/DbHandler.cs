using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using MyValidatorService.Database;
using MyValidatorService.Interfaces;
using NLog;

namespace MyValidatorService.Models
{
    public class DbHandler : IDbHandler
    {
        private readonly UserInformationEntities _dbcontext = new UserInformationEntities();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public List<UserDetail> GetUserDetails()
        {
            try
            {
                return _dbcontext.UserDetails.ToList();
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }

        }

        public void SaveUserDetails(UserDetail userDetails)
        {
            try
            {
                _dbcontext.UserDetails.AddOrUpdate(userDetails);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex.Message);
                throw;
            }
        }
    }
}
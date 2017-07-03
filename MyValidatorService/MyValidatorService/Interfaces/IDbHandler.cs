using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyValidatorService.Database;

namespace MyValidatorService.Interfaces
{
    public interface IDbHandler
    {
        List<UserDetail> GetUserDetails();
        void SaveUserDetails(UserDetail userDetails);
    }
}
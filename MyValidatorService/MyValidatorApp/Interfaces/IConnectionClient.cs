using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyValidatorApp.Interfaces
{
    public interface IConnectionClient
    {
        object GetServiceResponse(string id);
        void CloseConnection();
        object GetSavedUserDetails();

    }
}
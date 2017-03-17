using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.RequestModel;
using DataLayer.Model;

namespace BusinessLayer.IRepository
{
    public interface ITestAuthenticationRepository
    {
        ReturnMessage SignIn(SignInModel req);
        ReturnMessage SignOut();
    }
}

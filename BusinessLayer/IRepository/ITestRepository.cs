using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Repository;
using BusinessLayer.RequestModel;
using BusinessLayer.ViewModel;
using DataLayer.Model;

namespace BusinessLayer.IRepository
{
    public interface ITestRepository : IGenericRepository<Cj_Customer>
    {
        ReturnMessage GetData();

        ReturnMessage Test(SymEncryptModel test);

        ReturnMessage SignIn(SignInModel req);
    }
}

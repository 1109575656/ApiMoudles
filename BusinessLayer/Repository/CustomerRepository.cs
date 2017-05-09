using BusinessLayer.BaseRepository;
using BusinessLayer.IRepository;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Repository
{
    public class CustomerRepository : GenericRepository<Customer>,ICustomerRepository
    {
        public ReturnMessage Add()
        {
            //测试添加
            //Customer customer = new Customer();
            //customer.LoginName = "3209575656";
            //customer.Password = "11109575656";
            //this.Insert(customer);
            //测试删除
            this.Delete(m => m.Id == 4);
            this.Save();
            return ReturnMessage.Success("", "成功");
        }
    }
}

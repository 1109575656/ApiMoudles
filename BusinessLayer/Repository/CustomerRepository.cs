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
        public ReturnMessage TestAdd()
        {
            Customer customer = new Customer();
            customer.LoginName = "3209575656";
            customer.Password = "11109575656";
            this.Insert(customer);
            if (this.Save() > 0)
            {
                return ReturnMessage.Success("添加成功！", "");
            }
            return ReturnMessage.Failed("添加失败！", "");
        }

        public ReturnMessage TestDelete()
        {
            this.Delete(m => m.Id == 4);
            this.Save();
            return ReturnMessage.Success("删除成功！", "");
        }

        public ReturnMessage TestSelect()
        {
            //var list = this.Query(m => m.Id == 3);  or  this.AsQuerable().Where(m => m.Id == 3)
            return ReturnMessage.Success("查询成功！", this.AsQuerable().Where(m => m.Id == 3));
        }

        public ReturnMessage TestUpdate()
        {
            var customer = this.FindOne(m => m.Id == 3);
            customer.LoginName = "1234";
            this.Save();
            return ReturnMessage.Success("修改成功！", "");
        }
    }
}

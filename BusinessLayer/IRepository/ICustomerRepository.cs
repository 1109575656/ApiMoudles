using BusinessLayer.BaseRepository;
using DataLayer.Model;
namespace BusinessLayer.IRepository
{
    public interface ICustomerRepository: IGenericRepository<Customer>
    {
        ReturnMessage TestAdd();
        ReturnMessage TestDelete();
        ReturnMessage TestUpdate();
        ReturnMessage TestSelect();
    }
}

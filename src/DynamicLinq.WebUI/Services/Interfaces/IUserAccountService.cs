using DynamicLinq.Models;

namespace DynamicLinq.Services.Interfaces
{
    public interface IUserAccountService
    {
        IQueryable<UserAccount> GetAll();
    }
}

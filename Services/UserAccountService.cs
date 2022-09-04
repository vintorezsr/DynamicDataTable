using DynamicLinq.Models;
using DynamicLinq.Services.Interfaces;

namespace DynamicLinq.Services
{
    public class UserAccountService : IUserAccountService
    {
        public IQueryable<UserAccount> GetAll()
        {
            var userAccounts = new[]
            {
                new UserAccount { Id = 1, Name = "Amie", Email = "amie@test.com" },
                new UserAccount { Id = 2, Name = "Anna", Email = "anna@test.com" },
                new UserAccount { Id = 3, Name = "Amanda", Email = "amanda@test.com" },
                new UserAccount { Id = 3, Name = "Baron", Email = "baron@test.com" },
                new UserAccount { Id = 3, Name = "Jacky", Email = "jacky@test.com" },
            };

            return userAccounts.AsQueryable();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Stock_Back.DAL.Models;

namespace Services
{
    public interface IAppDbContext
    {
        Task<User> FindAsync<User>(int id);
        // Other methods
    }

    public class FakeAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }

        public Task<User> FindAsync(int id)
        {
            var expectedUser = new User { Id = 1 };
            if (id == 1)
            {
                return Task.FromResult(expectedUser);
            }
            return Task.FromResult<User>(null);
        }

        internal object Setup(Func<object, object> value)
        {
            throw new NotImplementedException();
        }

        // Implement other methods of AppDbContext as needed for your test
    }
}

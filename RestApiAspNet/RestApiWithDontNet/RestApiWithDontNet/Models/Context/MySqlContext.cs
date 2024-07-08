using Microsoft.EntityFrameworkCore;

namespace RestApiWithDontNet.Models.Context
{
    public class MySqlContext: DbContext
    {

        public MySqlContext() { }

        public MySqlContext(DbContextOptions<MySqlContext> options): base(options){}

        public DbSet<User> Users { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;

namespace RestApiWithDontNet.Models.Context
{
    public class MySqlContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserAuth> UsersAuth { get; set; }

        public MySqlContext() { }

        public MySqlContext(DbContextOptions<MySqlContext> options): base(options){}

    }
}

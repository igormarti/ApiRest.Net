using RestApiWithDontNet.Models;
using RestApiWithDontNet.Models.Context;
using RestApiWithDontNet.Repository.Generic;

namespace RestApiWithDontNet.Repository.Impl
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private MySqlContext _context;

        public BookRepository(MySqlContext context):base(context) {
            _context = context;
        }
       
    }
}

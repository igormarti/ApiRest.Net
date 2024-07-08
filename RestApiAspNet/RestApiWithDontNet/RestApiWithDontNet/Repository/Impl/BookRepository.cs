using RestApiWithDontNet.Models;
using RestApiWithDontNet.Models.Context;

namespace RestApiWithDontNet.Repository.Impl
{
    public class BookRepository : IBookRepository
    {
        private MySqlContext _context;

        public BookRepository(MySqlContext context) {
            _context = context;
        }

        public Book FindById(long id)
        {
            return _context.Books.SingleOrDefault(u => u.Id.Equals(id));
        }

        public List<Book> FindAll()
        {
            return _context.Books.ToList();
        }

        public Book Create(Book book)
        {
            _context.Add(book);
            _context.SaveChanges();
            return book;
        }

        public Book Update(long id, Book newBook, Book oldBook)
        {
            newBook.Id = oldBook.Id;
            _context.Entry(oldBook).CurrentValues.SetValues(newBook);
            _context.SaveChanges();
            return newBook;
        }

        public void Delete(long id)
        {
            var result = FindById(id);
            _context.Books.Remove(result);
            _context.SaveChanges();
        }

        public bool Exists(long id)
        {
            return _context.Books.Any(u => u.Id.Equals(id));
        }
    }
}

using RestApiWithDontNet.Models;

namespace RestApiWithDontNet.Repository
{
    public interface IBookRepository
    {
        Book Create(Book book);

        Book Update(long id, Book newBook, Book oldBook);

        Book FindById(long id);

        List<Book> FindAll();

        void Delete(long id);

        bool Exists(long id);
    }
}

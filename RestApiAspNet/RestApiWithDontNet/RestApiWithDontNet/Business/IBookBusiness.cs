using RestApiWithDontNet.Models;

namespace RestApiWithDontNet.Business
{
    public interface IBookBusiness
    {
        Book Create(Book book);

        Book Update(long id, Book book);

        Book FindById(long id);

        List<Book> FindAll();

        void Delete(long id);
    }
}

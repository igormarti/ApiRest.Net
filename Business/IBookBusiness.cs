using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;

namespace RestApiWithDontNet.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);

        BookVO Update(long id, BookVO book);

        BookVO FindById(long id);

        List<BookVO> FindAll();

        void Delete(long id);
    }
}

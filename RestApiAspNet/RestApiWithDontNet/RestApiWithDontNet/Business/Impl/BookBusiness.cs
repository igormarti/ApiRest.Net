using RestApiWithDontNet.Models;
using RestApiWithDontNet.Repository;

namespace RestApiWithDontNet.Business.Impl
{
    public class BookBusiness : IBookBusiness
    {
        private IBookRepository _bookRepository;

        public BookBusiness(IBookRepository bookRepository) {
            _bookRepository = bookRepository;
        }

        public Book FindById(long id)
        {
            Book book = _bookRepository.FindById(id);
            if (book == null) {
                throw new Exception("Book não encontrado.");
            }
            return book;
        }

        public List<Book> FindAll()
        {
            return _bookRepository.FindAll();
        }

        public Book Create(Book book)
        {
            try
            {
                return _bookRepository.Create(book);
            } catch (Exception)
            {
                throw;
            }
        }

        public Book Update(long id, Book book)
        {
            try
            {
                Book bookOld = _bookRepository.FindById(id);

                if (bookOld == null) { 
                    throw new Exception("Book não encontrado");
                }
                
                return _bookRepository.Update(id, book, bookOld);
            }
            catch (Exception) {
                throw;
            }
        }

        public void Delete(long id)
        {
            try
            {

                if (!_bookRepository.Exists(id))
                {
                    throw new Exception("Book não encontrado");
                }

                _bookRepository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

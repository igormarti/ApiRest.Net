using RestApiWithDontNet.Data.Converter.Impl;
using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;
using RestApiWithDontNet.Repository;

namespace RestApiWithDontNet.Business.Impl
{
    public class BookBusiness : IBookBusiness
    {
        private readonly IBookRepository _bookRepository;
        private readonly BookParser _bookParser;

        public BookBusiness(IBookRepository bookRepository, BookParser bookParser) {
            _bookRepository = bookRepository;
            _bookParser = bookParser;
        }

        public BookVO FindById(long id)
        {
            Book book = _bookRepository.FindById(id);
            if (book == null) {
                throw new Exception("Book não encontrado.");
            }
            return _bookParser.Parse(book);
        }

        public List<BookVO> FindAll()
        {
            return _bookParser.Parse(_bookRepository.FindAll());
        }

        public BookVO Create(BookVO book)
        {
            try
            {
                return _bookParser.Parse(_bookRepository.Create(_bookParser.Parse(book)));
            } catch (Exception)
            {
                throw;
            }
        }

        public BookVO Update(long id, BookVO book)
        {

            Book bookOld = _bookRepository.FindById(id);

            if (bookOld == null) { 
                throw new Exception("Book não encontrado");
            }
                
            return _bookParser.Parse(_bookRepository.Update(id, _bookParser.Parse(book), bookOld));

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

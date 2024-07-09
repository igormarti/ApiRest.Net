using RestApiWithDontNet.Data.Converter.Contracts;
using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Models;
using System.Globalization;

namespace RestApiWithDontNet.Data.Converter.Impl
{
    public class BookParser : IParser<Book, BookVO>, IParser<BookVO, Book>
    {
        public BookVO? Parse(Book? input)
        {
            if (input == null) return null;
            return new BookVO 
            { 
                CodBook= input.Id,
                TitleBook= input.Title,
                AuthorBook= input.Author,
                PriceBook= input.Price,
                PublishDateBook=input.PublishDate.ToString("dd/MM/yyyy HH:mm:ss"),
            };
        }

        public Book? Parse(BookVO? input)
        {
            if (input == null) return null;
            return new Book()
            {
                Id = input.CodBook,
                Title = input.TitleBook,
                Author = input.AuthorBook,
                Price = input.PriceBook,
                PublishDate = DateTime.ParseExact(input.PublishDateBook, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            };
        }

        public List<Book?> Parse(List<BookVO?> input)
        {
            if (input == null) return null;
            return input.Select(item=>Parse(item)).ToList();
        }

        public List<BookVO?> Parse(List<Book?> input)
        {
            if (input == null) return null;
            return input.Select(item => Parse(item)).ToList();
        }
    }
}

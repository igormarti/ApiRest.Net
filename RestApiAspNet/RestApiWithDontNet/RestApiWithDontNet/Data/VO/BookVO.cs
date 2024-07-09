namespace RestApiWithDontNet.Data.VO
{
    public class BookVO
    {
        public long CodBook { get; set; }
        public string TitleBook { get; set; }
        public string AuthorBook { get; set; }
        public decimal PriceBook { get; set; }
        public string PublishDateBook { get; set; }

        public BookVO(){}

        public BookVO(int codBook, string titleBook, string authorBook, decimal priceBook, string publishDateBook)
        {
            CodBook = codBook;
            TitleBook = titleBook;
            AuthorBook = authorBook;
            PriceBook = priceBook;
            PublishDateBook = publishDateBook;
        }

        public override bool Equals(object? obj)
        {
            return obj is BookVO vO &&
                   CodBook == vO.CodBook;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CodBook, TitleBook, AuthorBook, PriceBook, PublishDateBook);
        }
    }
}

using RestApiWithDontNet.Hypermedia.Abstract;

namespace RestApiWithDontNet.Hypermedia.Filters
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();   

    }
}

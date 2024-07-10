using Microsoft.AspNetCore.Mvc.Filters;

namespace RestApiWithDontNet.Hypermedia.Abstract
{
    public interface IResponseEnricher
{
        bool CanEnrich(ResultExecutingContext context);
        Task Enrich(ResultExecutingContext context);
    }
}

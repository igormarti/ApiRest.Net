using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using RestApiWithDontNet.Hypermedia.Abstract;
using System.Collections.Concurrent;

namespace RestApiWithDontNet.Hypermedia
{
    public abstract class ContentResponseEnriche<T> : IResponseEnricher where T : ISupportsHyperMedia
    {
        public bool CanEnrich(Type contentType)
        {
            return contentType == typeof(T) || contentType == typeof(List<T>);
        }

        protected abstract Task EnrichModel(T content, IUrlHelper urlHelper);

        bool IResponseEnricher.CanEnrich(ResultExecutingContext response)
        {
            if (response.Result is OkObjectResult okObjectResult)
            {
                return CanEnrich(okObjectResult.Value.GetType());
            }
            else if (response.Result is CreatedAtActionResult createdAtActionResult)
            {
                return CanEnrich(createdAtActionResult.Value.GetType());
            }

            return false;
        }

        public async Task Enrich(ResultExecutingContext response)
        {
            var urlHelper = new UrlHelperFactory().GetUrlHelper(response);
            if (response.Result is OkObjectResult okObjectResult)
            {
                await ProcessObject(okObjectResult, urlHelper);
            }
            else if(response.Result is CreatedAtActionResult createdAtActionResult)
            {
                await ProcessObject(createdAtActionResult, urlHelper);
            }
            await Task.FromResult<object>(null);
        }

        private async Task ProcessObject(ObjectResult obj, IUrlHelper urlHelper)
        {
            if (obj.Value is T model)
            {
                await EnrichModel(model, urlHelper);
            }
            else if (obj.Value is List<T> collection)
            {
                ConcurrentBag<T> bag = new ConcurrentBag<T>(collection);
                Parallel.ForEach(bag, (item) =>
                {
                    EnrichModel(item, urlHelper);
                }
                );
            }
        }


    }
}

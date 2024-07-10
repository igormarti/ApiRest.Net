using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestApiWithDontNet.Hypermedia.Filters
{
    public class HyperMediaFilter:ResultFilterAttribute //, IResultFilter
    {
        private readonly HyperMediaFilterOptions _hyperMediaFilterOptions;
        private readonly ILogger<HyperMediaFilter> _logger;

        public HyperMediaFilter(HyperMediaFilterOptions hyperMediaFilterOptions, ILogger<HyperMediaFilter> logger)
        {
            _hyperMediaFilterOptions = hyperMediaFilterOptions;
            _logger = logger;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            TryEnrichResult(context);
            base.OnResultExecuting(context);
        }

        private void TryEnrichResult(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okObjectResult || context.Result is CreatedAtActionResult createdAtActionResult)
            {
                var enricher = _hyperMediaFilterOptions.
                    ContentResponseEnricherList.
                        FirstOrDefault(x => x.CanEnrich(context));
                if (enricher != null) Task.FromResult(enricher.Enrich(context));
            }
        }

    }
}

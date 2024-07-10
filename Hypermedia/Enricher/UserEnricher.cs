using Microsoft.AspNetCore.Mvc;
using RestApiWithDontNet.Data.VO;
using RestApiWithDontNet.Hypermedia.Constants;
using System.Text;

namespace RestApiWithDontNet.Hypermedia.Enricher
{
    public class UserEnricher:ContentResponseEnriche<UserVO>
    {
        private readonly object _locker = new object();

        protected override Task EnrichModel(UserVO content, IUrlHelper urlHelper)
        {
            var path = "api/users";
            string link = GetLink(content.CodUser, urlHelper, path);
            int paramLink = link.LastIndexOf("/");
            string linkWithoutParam = link.Substring(0, paramLink);

            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.GET,
                Href = link,
                Rel = RelationType.self,
                Type = ResponseTypeFormat.DefaultGet
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.POST,
                Href = linkWithoutParam,
                Rel = RelationType.post,
                Type = ResponseTypeFormat.DefaultPost
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PUT,
                Href = link,
                Rel = RelationType.put,
                Type = ResponseTypeFormat.DefaultPut
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.PATCH,
                Href = link,
                Rel = RelationType.patch,
                Type = ResponseTypeFormat.DefaultPatch
            });
            content.Links.Add(new HyperMediaLink()
            {
                Action = HttpActionVerb.DELETE,
                Href = link,
                Rel = RelationType.delete,
                Type = "int"
            });

            return Task.CompletedTask;
        }

        private string GetLink(long codUser, IUrlHelper urlHelper, string path)
        {
            lock (_locker) {
                var url = new {controller = path, id = codUser};
                return new StringBuilder(urlHelper.Link("DefaultApi", url)).Replace("%2F","/").ToString();
            }
        }
    }
}

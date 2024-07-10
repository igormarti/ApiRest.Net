using System.Text;

namespace RestApiWithDontNet.Hypermedia
{
    public class HyperMediaLink
    {
        public string Rel {  get; set; }
        public string Href {
            get {
                object _lock = new object();
                lock (_lock) {
                    StringBuilder sb = new StringBuilder(href);
                    return sb.Replace("%2F", "/").ToString();
                }

            }
            set { href = value; } 
        }

        private string href;
        public string Type {  get; set; }
        public string Action {  get; set; }
    }
}

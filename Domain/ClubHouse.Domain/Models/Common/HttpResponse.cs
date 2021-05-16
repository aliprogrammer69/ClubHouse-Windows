using System.Collections.Generic;
using System.Net;

namespace ClubHouse.Domain.Models.Common {
    public class HttpResponse {
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; set; }
        public bool IsSuccess => StatusCode == HttpStatusCode.OK;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Mango.Web.SD;

namespace Mango.Web.Models
{
    public class ApiRequest
    {
        public HttpMethod httpMethod { get; set; } = HttpMethod.Get;
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}

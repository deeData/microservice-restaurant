using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Web
{
    //SD for static details
    public static class SD
    {
        public static string ProductAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public static string CouponAPIBase { get; set; }

        public enum ApiType 
        { 
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}

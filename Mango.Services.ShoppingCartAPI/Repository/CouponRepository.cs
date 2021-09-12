using Mango.Services.ShoppingCartAPI.Models.Dto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        //access the CouponAPI to check if coupon is still valid
        private readonly HttpClient client;
        //construtor will allow obj to use http client methods
        public CouponRepository(HttpClient client)
        {
            this.client = client;
        }

        public async Task<CouponDto> GetCoupon(string couponName)
        {
            var reponse = await client.GetAsync($"/api/coupon/{couponName}");
            var apiContent = await reponse.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return new CouponDto();
        }
    }
}

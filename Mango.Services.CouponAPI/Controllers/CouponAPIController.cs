using Mango.Services.CouponAPI.Models.Dto;
using Mango.Services.CouponAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponAPIController : Controller
    {
        protected ResponseDto _response;
        private ICouponRepository _couponRepository;
        
        public CouponAPIController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            this._response = new ResponseDto();
        }

     
        [HttpGet("{couponCode}")]
        public async Task<object> GetDiscountForCode(string couponCode)
        {
            try
            {
                var coupon = await _couponRepository.GetCouponByCode(couponCode);
                _response.Result = coupon;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


    }
}

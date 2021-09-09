using AutoMapper;
using Mango.Services.ShoppingCartAPI.DbContexts;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//ShoppingCart web interface functions
namespace Mango.Services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        //will always need dbContext to get db and Automapper
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        public CartRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Task<bool> ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);
            var productInDb = await _db.Products
                .FirstOrDefaultAsync( u => u.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);
            
            //if null, then product does not exist
            if (productInDb == null)
            {
                _db.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _db.SaveChangesAsync();
            }

            var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync( u => u.UserId == cart.CartHeader.UserId);
            if (cartHeaderFromDb == null)
            {
                _db.CartHeaders.Add(cart.CartHeader);
                await _db.SaveChangesAsync();
                //display details from new cart header id-- assign header id to details is
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _db.SaveChangesAsync();
            }
            else 
            {
                //set to no tracking in entityframeowrk
                var cartDetailsFromDb = await _db.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    u.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                if (cartDetailsFromDb == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
                else 
                {
                    cart.CartDetails.FirstOrDefault().Product = null;
                    //cartDetails exist therefore just update the count
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    _db.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
            }
            return _mapper.Map<CartDto>(cart);

        }

        public Task<CartDto> GetCartByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFromCart(int cartDetailsId)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Diagnostics.Eventing.Reader;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Resturant.services.Cart.DbContexts;
using Resturant.services.Cart.DTO;
using Resturant.services.Cart.Models;

namespace Resturant.services.Cart.Reposerty
{
    public class CartReposerty : ICartReposerty
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CartReposerty(AppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<CartDto> GetCartByUserID(string userId)
        {
            Models.Cart cart = new()
            {
                CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == userId)
            };
            cart.CartDetails = _context.CartDetails
                .Where(x => x.CartHeaderId == cart.CartHeader.CartHeaderId)
                .Include(x => x.Product);
            return _mapper.Map<CartDto>(cart);
        }
        public async Task<CartDto> CreateOrUpdateCart(CartDto cartDto)
        {
            Models.Cart cart = _mapper.Map<Models.Cart >(cartDto);

            //check if product exists in database, if not create it!
            var prodInDb = await _context.Products
                .FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.FirstOrDefault()
                .ProductId);
            if (prodInDb == null)
            {
                _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _context.SaveChangesAsync();
            }


            //check if header is null
            var cartHeaderFromDb = await _context.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);

            if (cartHeaderFromDb == null)
            {
                //create header and details
                _context.CartHeaders.Add(cart.CartHeader);
                await _context.SaveChangesAsync();
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                //if header is not null
                //check if details has same product
                var cartDetailsFromDb = await _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    //create details
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
                else
                {
                    //update the count / cart details
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    cart.CartDetails.FirstOrDefault().CartDetailId = cartDetailsFromDb.CartDetailId;
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _context.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDto>(cart);
        }
        public async Task<bool> RemoveProductFromCart(int cartDetailsId)
        {
            try
            {
                CartDetail cartDetail =
                    await _context.CartDetails.FirstOrDefaultAsync(x => x.CartDetailId == cartDetailsId);
                var TotalCount = _context.CartDetails.Where(x => x.CartHeaderId == cartDetail.CartHeaderId).Count();
                _context.CartDetails.Remove(cartDetail);
                if (TotalCount == 1)
                {
                    var HeadertoREmove =
                        await _context.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartDetail.CartHeaderId);
                    _context.Remove(HeadertoREmove);
                }

                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception exception)
            {
                return false;
            }
        }
        public async Task<bool> ApplyCoupon(string UserID, string CouponCode)
        {
            var cart =await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == UserID);
            cart.CouponCode = CouponCode;
            _context.CartHeaders.Update(cart);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveCoupon(string UserID)
        {
            var cart = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == UserID);
            cart.CouponCode = "";
            _context.CartHeaders.Update(cart);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ClearCart(string UserId)
        {
            var HeaderDromDb = await _context.CartHeaders.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (HeaderDromDb != null)
            {
                //remove range because it ienuramble
                _context.CartDetails.RemoveRange(_context.CartDetails.Where(x =>
                    x.CartHeaderId == HeaderDromDb.CartHeaderId));
                _context.CartHeaders.Remove(HeaderDromDb);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }


    }
}

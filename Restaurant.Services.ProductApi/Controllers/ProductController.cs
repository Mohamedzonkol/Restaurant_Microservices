using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Services.ProductApi.DTO;
using Restaurant.Services.ProductApi.Reposetries;

namespace Restaurant.Services.ProductApi.Controllers
{
    [Route("api/products")]
    public class ProductController : ControllerBase //because this is API
    {
        private readonly IProductReposetry _productReposetry;
        protected ResponseDto _response;
        public ProductController(IProductReposetry productReposetry)
        {
            _productReposetry = productReposetry;
            this._response = new ResponseDto();
        }

        [HttpGet]
        public async Task<object> Get() //can be responseDto بدل object
        {
            try
            {
                IEnumerable<ProdutDto> produtDtos = await _productReposetry.GetProducts();
                _response.Result= produtDtos;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMassages = new List<string>(){e.ToString()};
            }
            return _response;
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<object> Get(int id) //can be responseDto بدل object
        {
            try
            {
                ProdutDto produtDto = await _productReposetry.GetProductById(id);
                _response.Result= produtDto;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMassages = new List<string>(){e.ToString()};
            }
            return _response;
        }
        [HttpPost]
        [Authorize]
        public async Task<object> Post([FromBody]ProdutDto product)
        {
            try
            {
                ProdutDto model = await _productReposetry.CreateUpdateProduct(product);
                _response.Result= model;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMassages = new List<string>(){e.ToString()};
            }
            return _response;
        }
        [HttpPut]
        [Authorize]
        public async Task<object> Update([FromBody]ProdutDto product)
        {
            try
            {
                ProdutDto model = await _productReposetry.CreateUpdateProduct(product);
                _response.Result= model;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMassages = new List<string>(){e.ToString()};
            }
            return _response;
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool isSuccess = await _productReposetry.DeleteProduct(id);
                _response.Result= isSuccess;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMassages = new List<string>(){e.ToString()};
            }
            return _response;
        }
    }
}

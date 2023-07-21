namespace Shop.Web.Controllers.API
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Data;
    using Helper;
    using Data.Entities;
    using System.IO;

    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository productsRepository;
        private readonly ILogger<ProductsController> logger;
        private readonly IUserHelper userHelper;

        public ProductsController(
            IProductsRepository productsRepository, 
            ILogger<ProductsController> logger,
            IUserHelper userHelper)
        {
            this.productsRepository = productsRepository;
            this.logger = logger;
            this.userHelper = userHelper;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
                return this.Ok(this.productsRepository.GetAllWithUsers());
            }
            catch (Exception ex)
            {
                var message = $"Error: {ex}";
                this.logger.LogError(message);
                return this.BadRequest(message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] Common.Models.Product product)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var user = await this.userHelper.GetUserByEmailAsync(product.User.UserName);
            if (user == null)
            {
                return this.BadRequest("Invalid user");
            }

            var imageUrl = string.Empty;
            if (product.ImageArray != null && product.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(product.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "wwwroot\\images\\Products";
                var fullPath = $"~/images/Products/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    imageUrl = fullPath;
                }
            }

            var entityProduct = new Product
            {
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = user,
                ImageUrl = imageUrl
            };

            var newProduct = await this.productsRepository.CreateAsync(entityProduct);
            return Ok(newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] int id, [FromBody] Common.Models.Product product)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            var oldProduct = await this.productsRepository.GetByIdAsync(id);
            if (oldProduct == null)
            {
                return this.BadRequest("Product Id don't exists.");
            }

            //TODO: Upload images
            oldProduct.IsAvailabe = product.IsAvailabe;
            oldProduct.LastPurchase = product.LastPurchase;
            oldProduct.LastSale = product.LastSale;
            oldProduct.Name = product.Name;
            oldProduct.Price = product.Price;
            oldProduct.Stock = product.Stock;

            var updatedProduct = await this.productsRepository.UpdateAsync(oldProduct);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var product = await this.productsRepository.GetByIdAsync(id);
            if (product == null)
            {
                return this.NotFound();
            }

            await this.productsRepository.DeleteAsync(product);
            return Ok(product);
        }


    }
}

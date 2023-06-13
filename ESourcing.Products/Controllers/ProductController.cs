using ESourcing.Products.Entities;
using ESourcing.Products.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ESourcing.Products.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        #region Variables
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductController> _logger;
        #endregion
        #region Constructor
        public ProductController(IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }
        #endregion
        #region Crud_Actions
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)] //Birden fazla değeri aynı anda dönebilmek için Microsoftun tanımladığı bir attribute
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)", Name = "GetProduct")] //Parametre olarak 24 karakterli bir id gelecek ve metodun adı GetProduct olarak görünecek.
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if(product == null)
            {
                _logger.LogError($"Product with id : {id},hasn't been found database");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.Create(product);
            //Create ettikten sonra GetProduct a dön ve parametre olarak da belirtilen ıd değerini al. Alacağı ıd değeri oluşturulam productın id si olsun ve product da verilsin.
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.Update(product));
        }

        [HttpDelete("{id:length(24)")] //Parametre olarak 24 karakterli bir id gelecek ve metodun adı GetProduct olarak görünecek.
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteProductById(string id)
        {
            var product = await _productRepository.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product with id : {id},hasn't been found database");
                return NotFound();
            }
            return Ok(await _productRepository.Delete(id));
        }
        #endregion
    }
}

using DutchTreat.Models;
using DutchTreat.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IDutchRepository _dutchRepo;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IDutchRepository dutchRepo , ILogger<ProductsController> logger)
        {
            _dutchRepo = dutchRepo;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            try
            {
                return Ok(_dutchRepo.GetAllProducts());
            }

            catch (Exception ex)
            {
                _logger.LogError($"Failed To Get Product : {ex}");
                return BadRequest("Failed to Get Products");
            }
        }
    }
}

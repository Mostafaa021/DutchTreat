using AutoMapper;
using DutchTreat.DTOs;
using DutchTreat.Models;
using DutchTreat.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)] // Adding Authentication to this controller
    public class OrdersController : ControllerBase
    {
        private readonly IDutchRepository _dutchRepo;
        private readonly ILogger<OrdersController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<StoreUser> _userManager;

        public OrdersController(IDutchRepository dutchRepo,
            ILogger<OrdersController>logger ,
            IMapper mapper
            ,UserManager<StoreUser> userManager) 
        {
            _dutchRepo = dutchRepo;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult GetOrders( bool includeItems = true) 
        {
            try
            {
                // Adding user.identity.name to method to make this user only authorize to use this action 
                
                var username = User?.Identity?.Name; 
                var result = _dutchRepo.GetAllOrdersByUserName(username, includeItems);
                return Ok(_mapper.Map<IEnumerable<OrderDTO>>(result));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to Get Orders : {ex}");
                return BadRequest("Failed to Get Orders");
            }
            
        }
        [HttpGet("{id:int}")]
        public IActionResult GetOrder(int id)
        {
            try
            {
                var order = _dutchRepo.GetOrderById(User?.Identity?.Name,id);
                if (order != null) return Ok(_mapper.Map<Order,OrderDTO> (order));
                else return NotFound();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Get Orders : {ex}");
                return BadRequest("Failed to Get Orders");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromBody]OrderDTO model)
        {
            //add to the database    // using identity in post method to know the owner of order
            try
            {
                if (ModelState.IsValid)
                {
                    Order NewOrder = _mapper.Map<OrderDTO,Order>(model); // maping coming modelDTO to concrete model

                    if (NewOrder.OrderDate == DateTime.MinValue)
                    {
                        NewOrder.OrderDate = DateTime.Now;
                    }
                    // before adding entity 
                    // get current user                        // User here just list of claims for our user not the database represantion of our created user               
                    var CurrentUser = await _userManager.FindByNameAsync(User?.Identity?.Name);
                     // assiogn current user to concrete class (order) user 
                    NewOrder.User = CurrentUser;
                      // add entity to database 
                    _dutchRepo.AddEntity(NewOrder);
                    if (_dutchRepo.SaveAll())
                    {
                          // map from model to Model DTO
                        return Created($"/api/orders/{NewOrder.Id}", _mapper.Map<Order, OrderDTO>(NewOrder));
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to Add New Order {ex}");
                return BadRequest("Failed to Add New Order ");
            }
            return BadRequest("Failed to Add New Order");
        }
    }
}

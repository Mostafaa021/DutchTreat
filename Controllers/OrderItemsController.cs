using AutoMapper;
using DutchTreat.DTOs;
using DutchTreat.Models;
using DutchTreat.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    [Route("/api/orders/{orderid}/items")]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : ControllerBase
    {
        private readonly IDutchRepository _dutchRepo;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController( IDutchRepository dutchRepo ,
            ILogger<OrderItemsController>logger ,
            IMapper mapper)
        {
            _dutchRepo = dutchRepo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int orderId)
        {
            // Adding user.identity.name to method to make this user only authorize to use this action 
            var order = _dutchRepo.GetOrderById(User?.Identity?.Name, orderId);
            if(order != null)  return Ok(_mapper.Map<IEnumerable<OrderItem>,IEnumerable<OrderItemDTO>>(order.Items));
            return NotFound();
           
        }
        [HttpGet("{id:int}")]
        public IActionResult Get(int orderId , int id)
        { 
            // Adding user.identity.name to method to make this user only authorize to use this action 
            var order = _dutchRepo.GetOrderById(User?.Identity?.Name, orderId);
            if (order != null)
            {
                var item =  order.Items.Where(o => o.Id == id).FirstOrDefault();
                if(item != null)
                return 
                        Ok(_mapper.Map<OrderItem,OrderItemDTO>(item));
                else
                    return NotFound();
            }   
            return NotFound();

        }
    }
}

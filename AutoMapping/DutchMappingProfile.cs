using AutoMapper;
using DutchTreat.DTOs;
using DutchTreat.Models;

namespace DutchTreat.AutoMapping
{
    public class DutchMappingProfile : Profile
    {
        public DutchMappingProfile() 
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(o=>o.OrderId,ex=>ex.MapFrom(o=>o.Id))
                .ReverseMap(); // reverse process of mapping in two way direction between order and orderDto
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(o=>o.OrderItemId, ex=>ex.MapFrom(o=>o.Id))
                .ForMember(o=>o.OrderItemQuantity ,ex=>ex.MapFrom(o=>o.Quantity))
                .ForMember(o=>o.OrderItemUnitPrice, ex=>ex.MapFrom(o=>o.UnitPrice))
                .ReverseMap(); 
        }
    }
}

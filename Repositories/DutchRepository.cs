using DutchTreat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Repositories
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Get All Products was Called");
                return _context.Products
                                .OrderBy(p => p.Title)
                                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError( $"Failed to Get All Products : {ex}");
                return Enumerable.Empty<Product>(); 
            }
        }
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _context.Products
                            .Where(p => p.Category == category)
                            .ToList();
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                try
                {
                    _logger.LogInformation(" Get All Orders called");
                    return _context.Orders
                        .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                        .OrderBy(o => o.OrderNumber).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Get Orders");
                    return Enumerable.Empty<Order>();
                }
            }
            else
                return _context.Orders.ToList();
        }
        public IEnumerable<Order> GetAllOrdersByUserName(string username, bool includeItems)
        {
            if (includeItems)
            {
                try
                {
                    _logger.LogInformation(" Get All Orders called");
                    return _context.Orders
                        .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                        .Where(o=>o.User.UserName==username)
                        .OrderBy(o => o.OrderNumber).ToList();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to Get Orders");
                    return Enumerable.Empty<Order>();
                }
            }
            else
                return _context.Orders
                    .Where(o=>o.User.UserName == username)
                    .ToList();
        }
        public Order GetOrderById(string? username, int id)
        {
            return _context.Orders
                 .Include(o=>o.Items)
                 .ThenInclude(i => i.Product)
                 .Where(o => o.Id == id && o.User.UserName == username)
                 .First(); 
        }
        

        public void AddEntity(object model)
        {
            _context.Add(model);
        }
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

       
    }
}

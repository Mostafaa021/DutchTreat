using DutchTreat.Models;

namespace DutchTreat.Repositories
{
    public interface IDutchRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        IEnumerable<Order> GetAllOrders(bool includeItems);
        IEnumerable<Order> GetAllOrdersByUserName(string username , bool includeItems);

        Order GetOrderById(string? username , int id);
        void AddEntity(object model);
        bool SaveAll();
       
    }
}
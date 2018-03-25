using System.Collections.Generic;
using SimpleWebCore.Data.Entities;

namespace SimpleWebCore.Data
{
    public interface ISimpleWebCoreRepository
    {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int Id);
        void AddEntity(object model);
        bool SaveAll();
        void AddOrder(Order newOrder);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleWebCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebCore.Data
{
    public class SimpleWebCoreRepository : ISimpleWebCoreRepository
    {
        private readonly SimpleWebCoreContext _ctx;
        private readonly ILogger<SimpleWebCoreRepository> _logger;

        public SimpleWebCoreRepository(SimpleWebCoreContext ctx, ILogger<SimpleWebCoreRepository> logger) {
            _ctx = ctx;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product).ToList();
        }

        public IEnumerable<Product> GetAllProducts() {
            try
            {
                _logger.LogInformation("GetAllProducts was called");
                return _ctx.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex) {
                _logger.LogError("Failed to get All products: {0}", ex.Message);
                return null;
            }
        }

        public Order GetOrderById(int Id)
        {
            return _ctx.Orders.Include(o => o.Items).ThenInclude(i => i.Product).Where(o => o.Id == Id).FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _ctx.Products.Where(p=> p.Categoty == category).OrderBy(p => p.Title).ToList();
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges()>0 ;
        }
    }
}

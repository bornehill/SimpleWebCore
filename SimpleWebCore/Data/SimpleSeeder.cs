using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SimpleWebCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebCore.Data
{
    public class SimpleSeeder
    {
        private readonly SimpleWebCoreContext _ctx;
        private readonly IHostingEnvironment _hosting;
        private readonly UserManager<StoreUser> _userManager;

        public SimpleSeeder(SimpleWebCoreContext ctx, IHostingEnvironment hosting, UserManager<StoreUser> userManager) {
            _ctx = ctx;
            _hosting = hosting;
            _userManager = userManager;
        }

        public async Task Seed() {
            _ctx.Database.EnsureCreated();

            var user = await _userManager.FindByEmailAsync("bornehill@hotmail.com");

            if (user == null)
            {
                user = new StoreUser
                {
                    FirstName = "Osborne",
                    LastName = "Hill",
                    UserName = "bornehill",
                    Email = "bornehill@hotmail.com"
                };
            
                var result = await _userManager.CreateAsync(user, "P4ss0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Failed to create default user");
                }
            }

            if (!_ctx.Products.Any()) {
                //Create simple data
                var filepath = Path.Combine(_hosting.ContentRootPath, "Data/products.json");
                var json = File.ReadAllText(filepath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);
                _ctx.Products.AddRange(products);

                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderNumber = "00001",
                    User = user,
                    Items = new List<OrderItem> {
                        new OrderItem{
                            Product = products.First(),
                            Quantity = 2,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                _ctx.Orders.Add(order);
                _ctx.SaveChanges();
            }
        }
    }
}

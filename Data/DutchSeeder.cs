using DutchTreat.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Text.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _dutchContext;
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext dutchContext ,
            IWebHostEnvironment environment , 
            UserManager<StoreUser> userManager)
        {
            _dutchContext = dutchContext;
            _environment = environment;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _dutchContext.Database.EnsureCreated();
            // get user by email 
            StoreUser user = await _userManager.FindByEmailAsync("mostafaIsmail@gmail.com");
            if (user == null)// if user not exist
            { //create object from user with default following data
                user = new StoreUser()
                {
                    FirstName = "Mostafa",
                    LastName = "Ismail",
                    Email = "mostafaIsmail@gmail.com",
                    UserName = "mostafaIsmail@gmail.com"
                };
                // then create user with default password 
               var result =   await _userManager.CreateAsync(user,"01122528755@Arsh");
                if (result != IdentityResult.Success) // didn`t create new ueser
                {
                    throw new InvalidOperationException("Could Not Create new User in Seeder");
                }
            }

            if(!_dutchContext.Products.Any())
            {
                //Need to Adding Data
                var filepath = Path.Combine(_environment.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filepath);
                var prodcuts = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
                _dutchContext.Products.AddRange(prodcuts);

                var order = _dutchContext.Orders.Where(o=>o.Id==1).FirstOrDefault();
                // if there are order then assign user to this order 
                if (order != null) // will always be as i make default one in modelbuilder in DutchContext
                {
                    order.User = user;
                    order.Items = new List<OrderItem>()
                    {
                        new OrderItem
                        {
                            Product = prodcuts.First(),
                            Quantity = 5 ,
                            UnitPrice = prodcuts.First().Price
                        }

                    };
                }
                _dutchContext.SaveChanges();

            }

        }
    }
}

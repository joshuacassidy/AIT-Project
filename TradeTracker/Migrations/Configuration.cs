namespace TradeTracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using TradeTracker.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Web.Security;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class Configuration : DbMigrationsConfiguration<TradeTracker.Models.TradeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TradeTracker.Models.TradeContext context)
        {
            var testUsers = new[]
                {
                new {
                    Email = "jc@g.com",
                    Password = "Jc12345*"
                },
                new {
                    Email = "joshc@g.com",
                    Password = "Jc12345*"
                },
                new {
                    Email = "jct@g.com",
                    Password = "Jc12345*"
                },
                new {
                    Email = "jctest@g.com",
                    Password = "Jc12345*"
                },
                new {
                    Email = "josht@g.com",
                    Password = "Jc12345*"
                },
                new {
                    Email = "joshtest@g.com",
                    Password = "Jc12345*"
                },
            }.ToList();



            var userContext = new TradeTracker.Models.ApplicationDbContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));

            foreach (var testUser in testUsers)
            {
                var user = userContext.Users.FirstOrDefault(u => u.Email == testUser.Email);
                if (user != null)
                {
                    userContext.Users.Remove(user);
                    userContext.SaveChanges();
                }
            }

            var PasswordHash = new PasswordHasher();

            foreach (var testUser in testUsers)
            {
                var email = testUser.Email;
                var password = testUser.Password;
                if (!userContext.Users.Any(u => u.UserName == email || u.Email == email))
                {
                    var newUser = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        PasswordHash = PasswordHash.HashPassword(password),
                        DateAdded = DateTime.Now
                    };

                    UserManager.Create(newUser);
                }
                userContext.SaveChanges();
            }

            foreach (var testUser in testUsers)
            {
                var user = userContext.Users.FirstOrDefault(u => u.Email == testUser.Email);
                if (user != null)
                {
                    var status = new List<Status>
                    {
                        new Status { Name = "Stolen", UserId = user.Id },
                        new Status { Name = "Sold", UserId = user.Id },
                        new Status { Name = "Given Away", UserId = user.Id },
                        new Status { Name = "Lost", UserId = user.Id },
                        new Status { Name = "Broken", UserId = user.Id },
                    };

                    status.ForEach(stat => context.Status.AddOrUpdate(s => s.UserId, stat));
                    context.SaveChanges();

                    var categories = new List<Category>
                    {
                        new Category { Name = "Decorations", UserId = user.Id },
                        new Category { Name = "Jewelery", UserId = user.Id },
                        new Category { Name = "Garden", UserId = user.Id },
                        new Category { Name = "Christmas", UserId = user.Id },
                    };

                    categories.ForEach(category => context.Categories.AddOrUpdate(c => c.UserId, category));
                    context.SaveChanges();

                    var products = new List<Product>
                    {
                        new Product {
                            Name = "Flower Pot",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Garden")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Garden")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Sold")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Sold")).FirstOrDefault(),
                            UserId = user.Id
                        },
                        new Product {
                            Name = "Shovel",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Garden")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Garden")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Sold")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Sold")).FirstOrDefault(),
                            UserId = user.Id
                        },
                        new Product {
                            Name = "Diamond Ring",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Jewelery")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Jewelery")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Sold")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Sold")).FirstOrDefault(),
                            UserId = user.Id
                        },
                        new Product {
                            Name = "Gold Ring",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Jewelery")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Jewelery")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Stolen")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Stolen")).FirstOrDefault(),
                            UserId = user.Id
                        },
                        new Product {
                            Name = "Silver Ring",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Jewelery")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Jewelery")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Lost")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Lost")).FirstOrDefault(),
                            UserId = user.Id
                        },
                        new Product {
                            Name = "Christmas Tree",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Christmas")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Christmas")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Sold")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Sold")).FirstOrDefault(),
                            UserId = user.Id
                        },
                        new Product {
                            Name = "Christmas Tree Lights",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Christmas")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Christmas")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Sold")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Sold")).FirstOrDefault(),
                            UserId = user.Id
                        },
                        new Product {
                            Name = "Christmas Tree Decorations",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Christmas")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Christmas")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Given Away")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Given Away")).FirstOrDefault(),
                            UserId = user.Id
                        },
                        new Product {
                            Name = "Candles",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Decorations")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Decorations")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Sold")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Sold")).FirstOrDefault(),
                            UserId = user.Id
                        },
                        new Product {
                            Name = "Painting",
                            Price = 5.55m,
                            CategoryId = (categories.Where(c => c.Name == "Decorations")).FirstOrDefault().Id,
                            Category = (categories.Where(c => c.Name == "Decorations")).FirstOrDefault(),
                            StatusId = (status.Where(s => s.Name == "Sold")).FirstOrDefault().Id,
                            Status = (status.Where(s => s.Name == "Sold")).FirstOrDefault(),
                            UserId = user.Id
                        }
                    };
                    products.ForEach(product => context.Products.AddOrUpdate(p => p.UserId, product));
                    context.SaveChanges();

                    var clients = new List<Client>
                    {
                        new Client {
                            Name = "Josh",
                            Email = "j@g.com",
                            Phone = "0831112226",
                            UserId = user.Id,
                            Products = products.Where(product => product.Name.Equals("Flower Pot")).ToList()
                        },
                        new Client {
                            Name = "Harry",
                            Email = "harry@gmail.com",
                            Phone = "0831112223",
                            UserId = user.Id,
                            Products = products.Where(product => product.Name.Equals("Candles")).ToList()
                        },
                        new Client {
                            Name = "Harvey",
                            Email = "Harvey@gmail.com",
                            Phone = "0831112224",
                            UserId = user.Id,
                            Products = products.Where(product => product.Name.Equals("Candles")).ToList()
                        },
                        new Client {
                            Name = "Henry",
                            Email = "Henry@gmail.com",
                            Phone = "0831112225",
                            UserId = user.Id,
                            Products = products.Where(product => product.Name.Equals("Christmas Tree Decorations") || product.Name.Equals("Christmas Tree")).ToList()
                        },
                        new Client {
                            Name = "Tim",
                            Email = "Tim@gmail.com",
                            Phone = "0831112226",
                            UserId = user.Id,
                            Products = products.Where(product => product.Name.Equals("Shovel")).ToList()
                        },
                        new Client {
                            Name = "Conor",
                            Email = "Conor@gmail.com",
                            Phone = "0831112228",
                            UserId = user.Id,
                            Products = products.Where(product => product.Name.Equals("Diamond Ring")).ToList()
                        }
                    };
                    clients.ForEach(client => context.Clients.AddOrUpdate(c => c.UserId, client));
                    context.SaveChanges();
                }
            }
        }
    }
}

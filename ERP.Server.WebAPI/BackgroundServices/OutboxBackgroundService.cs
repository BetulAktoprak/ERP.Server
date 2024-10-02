using ERP.Server.Domain.Contants;
using ERP.Server.Domain.Entities;
using ERP.Server.Infrastructure.Context;
using ERP.Server.WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace ERP.Server.WebAPI.BackgroundServices;

public sealed class OutboxBackgroundService : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //IHubContext<DataHub> hubContext = ServiceTool.ServiceProvider.GetRequiredService<IHubContext<DataHub>>();
            ApplicationDbContext context = ServiceTool.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var client = new MongoClient("mongodb+srv://admin:1@erpdb.gkhkq.mongodb.net/");
            var database = client.GetDatabase("ERPDb");

            #region Create First User
            //IMongoCollection<User> _userCollection = database.GetCollection<User>("users");
            //FilterDefinition<User> userFilter = Builders<User>.Filter.Eq("IsDeleted", false);
            //bool haveUser = await _userCollection.Find(userFilter).AnyAsync(stoppingToken);
            //if (!haveUser)
            //{
            //    HashingHelper.CreatePassword("1", out byte[] passwordSalt, out byte[] passwordHash);
            //    User user = new()
            //    {
            //        FirstName = "Betül",
            //        LastName = "Aktoprak",
            //        Email = "betul_aktoprak@hotmail.com",
            //        IsEmailConfirmed = true,
            //        IsActive = true,
            //        UserName = "admin",
            //        PasswordHash = passwordHash,
            //        PasswordSalt = passwordSalt
            //    };
            //    _userCollection.InsertOne(user);
            //}
            #endregion


            List<Outbox> outboxes =
                await context.Outboxes
                .Where(p => p.IsCompleted == false && p.TryCount < 3)
                .OrderBy(p => p.CreateAt)
                .ToListAsync(stoppingToken);

            foreach (Outbox item in outboxes)
            {
                if (item.OperationName == OperationNames.Create)
                {
                    if (item.TableName == TableNames.Product)
                    {
                        Product? product = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (product is not null)
                        {
                            try
                            {
                                IMongoCollection<Product> _collection = database.GetCollection<Product>("products");
                                _collection.InsertOne(product);

                                item.IsCompleted = true;
                                await context.SaveChangesAsync(stoppingToken);
                                //await hubContext.Clients.All.SendAsync("createProduct", product);
                            }
                            catch (Exception)
                            {
                                if (item.TryCount >= 3)
                                {
                                    await context.SaveChangesAsync(stoppingToken);
                                    continue;
                                }
                                item.TryCount++;
                            }
                        }
                    }
                    else if (item.TableName == TableNames.User)
                    {
                        User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);

                        if (user is not null)
                        {
                            try
                            {
                                IMongoCollection<User> _collection = database.GetCollection<User>("users");
                                _collection.InsertOne(user);

                                item.IsCompleted = true;
                                await context.SaveChangesAsync(stoppingToken);
                            }
                            catch (Exception)
                            {
                                if (item.TryCount >= 3)
                                {
                                    await context.SaveChangesAsync(stoppingToken);
                                    continue;
                                }

                                item.TryCount++;
                            }
                        }
                    }
                }
                else if (item.OperationName == OperationNames.Update)
                {
                    if (item.TableName == TableNames.Product)
                    {
                        Product? product = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (product is not null)
                        {
                            IMongoCollection<Product> _collection = database.GetCollection<Product>("products");
                            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<Product> update = Builders<Product>.Update
                                  .Set(p => p.Name, product.Name)
                                  .Set(p => p.Type, product.Type)
                                  .Set(p => p.UpdateAt, product.UpdateAt)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);
                            //await hubContext.Clients.All.SendAsync("updateProduct", product);
                        }
                    }
                    else if (item.TableName == TableNames.User)
                    {
                        User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (user is not null)
                        {
                            IMongoCollection<User> _collection = database.GetCollection<User>("users");
                            FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<User> update = Builders<User>.Update
                                  .Set(p => p.FirstName, user.FirstName)
                                  .Set(p => p.LastName, user.LastName)
                                  .Set(p => p.UserName, user.UserName)
                                  .Set(p => p.Email, user.Email)
                                  .Set(p => p.PasswordSalt, user.PasswordSalt)
                                  .Set(p => p.PasswordHash, user.PasswordHash)
                                  .Set(p => p.UpdateAt, user.UpdateAt)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);

                        }
                    }
                }
                else if (item.OperationName == OperationNames.Delete)
                {
                    if (item.TableName == TableNames.Product)
                    {
                        Product? product = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (product is not null)
                        {
                            IMongoCollection<Product> _collection = database.GetCollection<Product>("products");
                            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<Product> update = Builders<Product>.Update
                                  .Set(p => p.UpdateAt, product.UpdateAt)
                                  .Set(p => p.IsDeleted, product.IsDeleted)
                                  .Set(p => p.DeleteAt, product.DeleteAt)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);
                            //await hubContext.Clients.All.SendAsync("deleteProduct", product);
                        }
                    }
                    else if (item.TableName == TableNames.User)
                    {
                        User? user = await context.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Id == item.RecordId, stoppingToken);
                        if (user is not null)
                        {
                            IMongoCollection<User> _collection = database.GetCollection<User>("users");
                            FilterDefinition<User> filter = Builders<User>.Filter.Eq("_id", item.RecordId);
                            UpdateDefinition<User> update = Builders<User>.Update
                                  .Set(p => p.UpdateAt, user.UpdateAt)
                                  .Set(p => p.IsDeleted, user.IsDeleted)
                                  .Set(p => p.DeleteAt, user.DeleteAt)
                                  ;

                            UpdateResult updateResult = _collection.UpdateOne(filter, update);
                            if (updateResult.ModifiedCount <= 0)
                            {
                                item.TryCount++;
                            }
                            else
                            {
                                item.IsCompleted = true;
                            }

                            await context.SaveChangesAsync(stoppingToken);

                        }
                    }
                }
                await Task.Delay(300);
            }
            await Task.Delay(10000);
        }
    }
}

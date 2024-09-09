
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
            ApplicationDbContext context = ServiceTool.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var client = new MongoClient("mongodb+srv://admin:1@erpdb.gkhkq.mongodb.net/");
            var database = client.GetDatabase("ERPDb");

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
                        Product? product = await context.Products.FindAsync(item.RecordId, stoppingToken);
                        if (product is not null)
                        {
                            try
                            {
                                IMongoCollection<Product> _collection = database.GetCollection<Product>("products");
                                _collection.InsertOne(product);

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
                        Product? product = await context.Products.FindAsync(item.RecordId, stoppingToken);
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
                        }
                    }
                }
                else if (item.OperationName == OperationNames.Delete)
                {
                    if (item.TableName == TableNames.Product)
                    {
                        Product? product = await context.Products.FindAsync(item.RecordId, stoppingToken);
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
                        }
                    }
                }
                await Task.Delay(300);
            }
            await Task.Delay(10000);
        }
    }
}

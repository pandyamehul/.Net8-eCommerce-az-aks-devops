using MongoDB.Driver;
using MongoDB.Bson;
using eCommerce.OrderService.DataAccessLayer.Entities;
using eCommerce.OrderService.DataAccessLayer.RepositoryContracts;


namespace eCommerce.OrderService.DataAccessLayer.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly IMongoCollection<Order> _orders;
    private readonly IMongoDatabase _database;
    private readonly string collectionName = "orders";

    public OrdersRepository(IMongoDatabase mongoDatabase)
    {
        _database = mongoDatabase;
        _orders = mongoDatabase.GetCollection<Order>(collectionName);
        Console.WriteLine($"OrdersRepository initialized with database: {mongoDatabase.DatabaseNamespace.DatabaseName}");
    }


    public async Task<Order?> AddOrder(Order order)
    {
        order.OrderID = Guid.NewGuid();
        order._id = order.OrderID;

        foreach (OrderItem orderItem in order.OrderItems)
        {
            orderItem._id = Guid.NewGuid();
        }

        await _orders.InsertOneAsync(order);
        return order;
    }


    public async Task<bool> DeleteOrder(Guid orderID)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, orderID);

        Order? existingOrder = (await _orders.FindAsync(filter)).FirstOrDefault();

        if (existingOrder == null)
        {
            return false;
        }

        DeleteResult deleteResult = await _orders.DeleteOneAsync(filter);

        return deleteResult.DeletedCount > 0;
    }


    public async Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        return (await _orders.FindAsync(filter)).FirstOrDefault();
    }


    public async Task<IEnumerable<Order>> GetOrders()
    {
        try
        {
            Console.WriteLine($"Querying collection: {collectionName}");

            // First, try to get raw BsonDocuments to verify data exists
            var rawCollection = _database.GetCollection<BsonDocument>(collectionName);
            var rawCursor = await rawCollection.FindAsync(new BsonDocument());
            var rawDocs = await rawCursor.ToListAsync();
            Console.WriteLine($"Raw document count: {rawDocs.Count}");
            if (rawDocs.Count > 0)
            {
                Console.WriteLine($"First raw document: {rawDocs[0].ToJson()}");
            }

            // Now try with typed collection
            var filter = Builders<Order>.Filter.Empty;
            Console.WriteLine($"Filter: {filter}");

            var cursor = await _orders.FindAsync(filter);
            var orders = await cursor.ToListAsync();

            Console.WriteLine($"Retrieved {orders.Count} orders from MongoDB");

            if (orders.Count > 0)
            {
                Console.WriteLine($"First order: {orders[0].OrderID}");
            }

            return orders;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving orders: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }


    public async Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        return (await _orders.FindAsync(filter)).ToList();
    }


    public async Task<Order?> UpdateOrder(Order order)
    {
        FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, order.OrderID);

        Order? existingOrder = (await _orders.FindAsync(filter)).FirstOrDefault();

        if (existingOrder == null)
        {
            return null;
        }

        order._id = existingOrder._id; // preserve the original _id

        ReplaceOneResult replaceOneResult = await _orders.ReplaceOneAsync(filter, order);

        return order;
    }
}

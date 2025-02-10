using System.ComponentModel;
using Microsoft.Azure.Cosmos;
using Container = Microsoft.Azure.Cosmos.Container;

namespace WebApiOne.Api.Operations;

public class OperationHandler<T> : IOperationHandler<T> where T : CommonEntity
{
    private const string KeyVaultUri = $"https://onsachin-kv.vault.azure.net";

    private const string connectionString =
        "AccountEndpoint=https://onsachin-cosmosdb.documents.azure.com:443/;AccountKey=jK74PeYRLaPKJ5vlrXgT79384fZgM040mKqq2xrDqO6a61hjjhY7EIMgvRQHN9xHPLY5bkdGnijwACDbwC1dRg==;";

    private Task<CosmosClient> CreateCosmosClient()
    {
        // var client = new SecretClient(new Uri(KeyVaultUri), new DefaultAzureCredential());
        // var secret = await client.GetSecretAsync("cosmos-cstr");
        // string cstr = secret.Value.Value;
        //
        // try
        // {
        //     return new CosmosClient(cstr);
        // }
        // catch (Exception e)
        // {
        //     return new CosmosClient(connectionString);
        // }

        return Task.FromResult(new CosmosClient(connectionString));
    }

    public async Task<T> GetByIdAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        var type = entity as CommonEntity;
        var container = await GetContainer(typeof(T).Name);
        ItemResponse<T> response = await container.ReadItemAsync<T>(type.Id, new PartitionKey(type.PartitionKey));
        entity = response.Resource;
        return entity;
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        string sqlQueryText = $"SELECT * FROM c WHERE c.partitionKey = '{typeof(T).Name}' AND c.id='{id}'";
        var results = await GetByQueryAsync(sqlQueryText);
        return results.SingleOrDefault();
    }


    public async Task<T?> GetByEmailAsync(string email)
    {
        string sqlQueryText = $"SELECT * FROM c WHERE c.partitionKey ='{typeof(T).Name}' AND c.Email='{email}'";
        var results = await GetByQueryAsync(sqlQueryText);
        return results.SingleOrDefault() as T;
    }

    public async Task<IList<T>> GetByQueryAsync(string query)
    {
        var container = await GetContainer(typeof(T).Name);
        using FeedIterator<T> results = container.GetItemQueryIterator<T>(query);
        while (results.HasMoreResults)
        {
            FeedResponse<T> response = await results.ReadNextAsync();
            return response.Resource.ToList();
        }

        return new List<T>();
    }

    public async Task<IList<T>> GetAllAsync()
    {
        var container = await GetContainer(typeof(T).Name);
        FeedResponse<T> response = await container.ReadManyItemsAsync<T>(items: null);
        return response.Resource.ToList();
    }

    public async Task<T?> Insert(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        var container = await GetContainer(typeof(T).Name);
        if (container == null)
            throw new ArgumentNullException(nameof(container));
        var response = await container.CreateItemAsync(entity);
        return response.Resource;
    }

    public async Task<T> Update(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        var type = entity as T;
        var container = await GetContainer(typeof(T).Name);
        if (container == null)
            throw new ArgumentNullException(nameof(container));
        var response = await container.UpsertItemAsync(entity);
        return response.Resource;
    }

    public async Task<T> Delete(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        var container = await GetContainer(typeof(T).Name);
        if (container == null)
            throw new ArgumentNullException(nameof(container));
        var response = await container.DeleteItemAsync<T>(entity.Id, new PartitionKey(entity.PartitionKey));
        return response.Resource;
    }

    private async Task<Database> GetDatabase()
    {
        string databaseName = "APILogAnalytics";
        var client = await CreateCosmosClient();
        var foundDatabase = client.GetDatabase(databaseName);
        // if (foundDatabase != null)
        // return foundDatabase;
        DatabaseResponse response = await client.CreateDatabaseIfNotExistsAsync(databaseName);
        return response.Database;

    }

    private async Task<Container> GetContainer(string id)
    {
        var client = await CreateCosmosClient();
        var foundContainer = client.GetContainer("APILogs", id);
        // if (foundContainer!= null)
        //    return foundContainer;

        var database = await GetDatabase();
        var container = await database.CreateContainerIfNotExistsAsync(id: id, "/partitionKey");
        return container.Container;
    }
}

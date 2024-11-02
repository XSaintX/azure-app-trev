using Microsoft.Azure.Cosmos;
//using System.ComponentModel;

namespace BlazorCosmosDb.Demo.Data
{
    public class EngineerService : IEngineerService
    {
        private readonly string CosmosDBConnectionString = "AccountEndpoint=https://azure-xsaint-cosmosdb.documents.azure.com:443/;AccountKey=1HIUoPDH4644Wqi1nXdQNOL1VUH4P2sJzyAYF45Ht3mpFoe5p4YSTKVZz9rc9CvcaKcmJ7dAJ4zIACDbxGH28Q==;";

        private readonly string CosmosDBName = "Contractors";
        private readonly string CosmosDBContainerName = "Engineers";

        private Container GetContainerClient()
        {
            var cosmosDbClient = new CosmosClient(CosmosDBConnectionString);
            var container = cosmosDbClient.GetContainer(CosmosDBName, CosmosDBContainerName);
            return container;
        }

        public async Task UpsertEngineer(Engineer engineer)
        {
            try
            {
                if (engineer.Id == null)
                {
                    engineer.Id = Guid.NewGuid();
                }
                var container = GetContainerClient();
                var updateRes = await container.UpsertItemAsync(engineer, new PartitionKey(engineer.id.ToString()));
                Console.Write(updateRes.StatusCode);
            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }

        public async Task DeleteEngineer(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                var response = await container.DeleteItemAsync<Engineer>(id, new PartitionKey(partitionKey));
            }
            catch (Exception ex)
            {
                throw new Exception("Exception", ex);
            }
        }

        public async Task<List<Engineer>> GetEngineerDetails()
        {
            List<Engineer> engineers = new List<Engineer>();
            try
            {
                var container = GetContainerClient();
                var sqlQuery = "SELECT * FROM c";
                QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
                FeedIterator<Engineer> queryResultSetIterator = container.GetItemQueryIterator<Engineer>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<Engineer> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (Engineer engineer in currentResultSet)
                    {
                        engineers.Add(engineer);
                    }
                }
            }
            catch (Exception ex)
            {

                ex.Message.ToString();
            }
            return engineers;
        }

        public async Task<Engineer> GetEngineerDetailsById(string? id, string? partitionKey)
        {
            try
            {
                var container = GetContainerClient();
                ItemResponse<Engineer> response = await container.ReadItemAsync<Engineer>(id, new PartitionKey(partitionKey));
                return response.Resource;
            }
            catch (Exception ex)
            {

                throw new Exception("Exception ", ex);
            }
        }

    }
}

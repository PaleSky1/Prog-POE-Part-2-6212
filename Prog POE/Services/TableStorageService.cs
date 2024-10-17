using Azure;
using Azure.Data.Tables;
using Prog_POE.Models;

namespace Prog_POE.Services
{
    public class TableStorageService
    {
        private readonly TableClient _tableClaimClient;

        public TableStorageService(string connectionString)
        {
            _tableClaimClient = new TableClient(connectionString, "Claims");

            _tableClaimClient.CreateIfNotExists();
        }

        public async Task<List<Claims>> GetAllClaimsTableAsync()
        {
            var claim = new List<Claims>();
            await foreach (var claims in _tableClaimClient.QueryAsync<Claims>())
            {
                claim.Add(claims);
            }
            return claim;
        }
        public async Task addClaimsAsync(Claims claims)
        {
            if (string.IsNullOrEmpty(claims.PartitionKey) || string.IsNullOrEmpty(claims.RowKey))
            {
                throw new ArgumentException("PartionKey and RowKey must be set");
            }
            try
            {
                await _tableClaimClient.AddEntityAsync(claims);
            }
            catch (RequestFailedException ex)
            {
                throw new ArgumentException("PartionKey and RowKey must be set", ex);
            }
        }
    }
}
using Azure;
using Azure.Data.Tables;
using Prog_POE.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prog_POE.Services
{
    public class TableStorageService
    {
        private readonly TableClient _tableClaimsClient;
        private readonly TableClient _tableUsersClient;

        public TableStorageService(string connectionString)
        {
            _tableClaimsClient = new TableClient(connectionString, "Claims");
            _tableUsersClient = new TableClient(connectionString, "Users");

            _tableClaimsClient.CreateIfNotExists();
            _tableUsersClient.CreateIfNotExists();
        }
        public async Task<User?> GetUserAsync(string username)
        {
            try
            {
                var response = await _tableUsersClient.GetEntityAsync<User>("User", username);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task AddUserAsync(User user)
        {
            if (string.IsNullOrEmpty(user.RowKey))
            {
                throw new ArgumentException("RowKey must be set");
            }

            try
            {
                await _tableUsersClient.AddEntityAsync(user);
            }
            catch (RequestFailedException ex)
            {
                throw new ArgumentException("Error adding user to Azure Table Storage", ex);
            }
        }

        public async Task<List<Claims>> GetAllClaimsAsync()
        {
            var claims = new List<Claims>();
            await foreach (var claim in _tableClaimsClient.QueryAsync<Claims>())
            {
                claims.Add(claim);
            }
            return claims;
        }

        public async Task AddClaimAsync(Claims claim)
        {
            if (string.IsNullOrEmpty(claim.PartitionKey) || string.IsNullOrEmpty(claim.RowKey))
            {
                throw new ArgumentException("PartitionKey and RowKey must be set");
            }

            try
            {
                await _tableClaimsClient.AddEntityAsync(claim);
            }
            catch (RequestFailedException ex)
            {
                throw new ArgumentException("Error adding entity to Azure Table Storage", ex);
            }
        }

        public async Task DeleteClaimAsync(string partitionKey, string rowKey)
        {
            await _tableClaimsClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<Claims?> GetClaimAsync(string partitionKey, string rowKey)
        {
            try
            {
                var response = await _tableClaimsClient.GetEntityAsync<Claims>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Elsa.EntityFrameworkCore.Modules.Identity;
using FSH.Starter.ElsaWorkflow.Infrastructure.Auth.ApiKey;
using FSH.Starter.ElsaWorkflow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Auth.ApiKey;

public class DbApiKeyStore : IApiKeyStore
{
    private readonly IDbContextFactory<ElsaStoreDbContext> _dbContextFactory;

    public DbApiKeyStore(IDbContextFactory<ElsaStoreDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ApiKey?> GetApiKeyAsync(string key)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        return await dbContext.ApiKeys
            .FirstOrDefaultAsync(x => x.Key == key);
    }

    public async Task<ApiKey> CreateApiKeyAsync(string owner, List<string> roles, DateTime? expires = null)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var apiKey = new ApiKey
        {
            Key = GenerateApiKey(),
            Owner = owner,
            Created = DateTime.UtcNow,
            Expires = expires,
            Roles = roles,
            IsActive = true
        };

        dbContext.ApiKeys.Add(apiKey);
        await dbContext.SaveChangesAsync();

        return apiKey;
    }

    public async Task RevokeApiKeyAsync(string key)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

        var apiKey = await dbContext.ApiKeys.FirstOrDefaultAsync(x => x.Key == key);
        if (apiKey != null)
        {
            apiKey.IsActive = false;
            await dbContext.SaveChangesAsync();
        }
    }

    private static string GenerateApiKey()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Persistence;
using FSH.Starter.WebApi.Document.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Infrastructure.Persistence;
internal sealed class DocumentDbInitializer(
    ILogger<DocumentDbInitializer> logger,
    DocumentDbContext context) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for document module", context.TenantInfo!.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        const string Name = "Document 01";
        const string Description = "A test Document 01";
        if (await context.Documents.FirstOrDefaultAsync(t => t.Name == Name, cancellationToken).ConfigureAwait(false) is null)
        {
            var document = Domain.Document.Create(Name, Description);
            await context.Documents.AddAsync(document, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] seeding default document data", context.TenantInfo!.Identifier);
        }
        StorageAccount storageAccount = null;
        if (await context.StorageAccounts.AnyAsync().ConfigureAwait(false))
        {
            storageAccount = StorageAccount.Create(StorageProvider.AmazonS3 ,"AWS","","", "Amazon Web Services Storage Account");
            await context.StorageAccounts.AddAsync(storageAccount, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        }

        if (storageAccount is not null && await context.Buckets.AnyAsync().ConfigureAwait(false))
        {
            var bucket = Bucket.Create(storageAccount, "us-east-1", "akhbar-demo", "arn:aws:s3:::akhbar-demo", "My Application Bucket", 0, 0);
            bucket.Folders.Add(Domain.Folder.Create(bucket));
            await context.Buckets.AddAsync(bucket, cancellationToken);
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using FSH.Starter.WebApi.Document.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.CreateFolder.v1;
public sealed class CreateBucketFolderHandler(
        ILogger<CreateBucketFolderHandler> logger,
        IStorageServiceFactory serviceFactory,
        [FromKeyedServices("document:buckets")] IRepository<Bucket> repository
    ) : IRequestHandler<CreateBucketFolderCommand,CreateBucketFolderResponse> {


    public Task<CreateBucketFolderResponse> Handle(CreateBucketFolderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Persistence;
using FSH.Framework.Core.Storage;
using FSH.Framework.Core.Storage.File;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.AddFolder.v1;
public sealed class AddBucketFolderHandler(
        ILogger<AddBucketFolderHandler> logger,
        IStorageServiceFactory serviceFactory,
        IRepository<Domain.Folder> repository
    ) : IRequestHandler<AddBucketFolderCommand,AddBucketFolderResponse> {


    public Task<AddBucketFolderResponse> Handle(AddBucketFolderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

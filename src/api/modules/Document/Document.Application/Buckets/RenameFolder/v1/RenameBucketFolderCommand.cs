using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.RenameFolder.v1;

public sealed record RenameBucketFolderCommand
(
     Guid BucketId,
     Guid FolderId,
     string NewFolderName
) : IRequest<RenameBucketFolderResponse>;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.RenameFile.v1;

public sealed record RenameBucketFileCommand(
     Guid BucketId,
     Guid FolderId,
     Guid FileId,
     string NewFileName
) : IRequest<RenameBucketFileResponse>;

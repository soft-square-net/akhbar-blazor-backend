using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Storage.File;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.AddFolder.v1;
public sealed record AddBucketFolderCommand(
    Guid BucketId,
    Guid ParentFolderId,
    string FolderName,
    string Description):IRequest<AddBucketFolderResponse>;

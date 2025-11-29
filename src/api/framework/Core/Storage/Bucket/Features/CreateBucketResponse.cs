using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Framework.Core.Storage.Bucket.Features;
public record SvcCreateBucketResponse(
    string BucketName,
    string ResourceName,
    string Location,
    IDictionary<string, string> Metadata
);

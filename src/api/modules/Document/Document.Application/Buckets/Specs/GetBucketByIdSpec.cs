using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.Specs;
public class GetBucketByIdSpec: SingleResultSpecification<Bucket>
{
    public GetBucketByIdSpec(Guid id)
    {
        Query
            .Include(e => e.StorageAccount)
            .Include(e => e.Folders)
            .ThenInclude(e =>e.Files)
            .Where(b => b.Id == id);
    }
}

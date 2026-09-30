using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Application.Buckets.Specs;
public class GetBucketByIdSpec: SingleResultSpecification<Bucket>
{
    public GetBucketByIdSpec(Guid id)
    {
        Query
            .Include(e => e.StorageAccount)
            .Include(e => e.Folders).ThenInclude(f => f.Files)
            .Include(b =>b.Folders).ThenInclude(f => f.Children)
            .Where(b => b.Id == id);
    }
}

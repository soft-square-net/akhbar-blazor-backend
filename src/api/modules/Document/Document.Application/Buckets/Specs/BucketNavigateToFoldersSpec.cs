using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.Create.v1;
public class BucketNavigateToFoldersSpec : SingleResultSpecification<Bucket>
{
    public BucketNavigateToFoldersSpec(Guid id,Guid? folderId = default)
    {

        if (folderId is null)
        {
            Query
                .Include(b => b.StorageAccount)
                .Where(b => b.Id == id)
                .Include(b => b.Folders.Where(f => f.Parent == null))
                .ThenInclude(f => f.Files)
                .Include(b => b.Folders.Where(f => f.Parent == null))
                .ThenInclude(f => f.Children);

        }
        else
        {
            Query
                .Include(b => b.StorageAccount)
                .Where(b => b.Id == id)
                .Include(b => b.Folders.Where(f => f.Id == folderId))
                .ThenInclude(f => f.Files)
                .Include(b => b.Folders.Where(f => f.Id == folderId))
                .ThenInclude(f => f.Children);
        }
    }
}

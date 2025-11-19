using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
public class GetDocumentRequest : IRequest<DocumentResponse>
{
    public Guid Id { get; set; }
    public GetDocumentRequest(Guid id) => Id = id;
}

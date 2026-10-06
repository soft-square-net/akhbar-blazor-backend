using MediatR;

namespace FSH.Starter.WebApi.Keyword.Application.Documents.ProcessDocumentAnalysis.v1;

//public class ProcessDocumentAnalysisCommand : IRequest<ProcessDocumentAnalysisResponse>
//{
//}
public record ProcessDocumentAnalysisCommand(int DocumentId) : IRequest<bool>;

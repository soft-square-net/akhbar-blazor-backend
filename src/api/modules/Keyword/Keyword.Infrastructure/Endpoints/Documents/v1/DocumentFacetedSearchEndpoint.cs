using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using FSH.Starter.WebApi.Keyword.Application.Documents.FacetedSearch.v1;

namespace FSH.Starter.WebApi.Keyword.Infrastructure.Endpoints.Documents.v1;

internal sealed class DocumentFacetedSearchEndpoint(
    string? Keyword = null,
    string? Keyphrase = null,
    string? PartOfSpeech = null,
    string? EntityType = null,
    int PageNumber = 1,
    int PageSize = 10) : IRequest<DocumentFacetedSearchResponse>;

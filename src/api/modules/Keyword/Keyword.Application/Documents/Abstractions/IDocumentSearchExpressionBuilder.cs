using System;
using System.Linq.Expressions;
using FSH.Starter.WebApi.Keyword.Domain;

namespace FSH.Starter.WebApi.Keyword.Application.Documents.Abstractions;

public interface IDocumentSearchExpressionBuilder
{
    Expression<Func<Document, bool>>? BuildSearchExpression(string? searchTerm);
    Expression<Func<Document, double>>? BuildRankExpression(string? searchTerm);
}

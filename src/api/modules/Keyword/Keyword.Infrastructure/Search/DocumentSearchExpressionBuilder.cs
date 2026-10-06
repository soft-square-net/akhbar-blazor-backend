using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using FSH.Starter.WebApi.Keyword.Application.Documents.Abstractions;
using FSH.Starter.WebApi.Keyword.Domain;
using FSH.Starter.WebApi.Keyword.Infrastructure.Persistence;

namespace FSH.Starter.WebApi.Keyword.Infrastructure.Search;

public class DocumentSearchExpressionBuilder : IDocumentSearchExpressionBuilder
{
    private readonly KeywordDbContext _dbContext;

    public DocumentSearchExpressionBuilder(KeywordDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Expression<Func<Document, bool>>? BuildSearchExpression(string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return null;

        var term = searchTerm.Trim();

        // 1. PostgreSQL Strategy using EF.Property for Shadow Vector Column
        if (_dbContext.Database.IsNpgsql())
        {
            /*return d => EF.Functions
                .ToTsVector("english", "SearchVector")
                .Matches(EF.Functions.WebSearchToTsQuery("english", term)) ;*/
            /*return d => EF.Functions.WebSearchToTsQuery("english", term)
                .Matches(EF.Property<NpgsqlTsVector>(d, "SearchVector"));*/
            return d => EF.Property<NpgsqlTsVector>(d, "SearchVector")
                .Matches(EF.Functions.WebSearchToTsQuery("english", term));
        }

        // 2. MSSQL Strategy
        if (_dbContext.Database.IsSqlServer())
        {
            return d => EF.Functions.FreeText(d.Title, term)
                        || EF.Functions.FreeText(d.Content, term);
        }

        // 3. Fallback (SQLite / In-Memory / Generic SQL)
        var lowerTerm = term.ToLower();
        return d => EF.Functions.Like(d.Title.ToLower(), $"%{lowerTerm}%")
                    || EF.Functions.Like(d.Content.ToLower(), $"%{lowerTerm}%");
    }
    
    public Expression<Func<Document, double>>? BuildRankExpression(string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return null;

        var term = searchTerm.Trim();

        // 1. PostgreSQL: Native ts_rank execution over the shadow vector column
        if (_dbContext.Database.IsNpgsql())
        {
            // return d => (double)EF.Functions.TsRank(
            //     EF.Property<NpgsqlTsVector>(d, "SearchVector"),
            //     EF.Functions.WebSearchToTsQuery("english", term));
            return d => (double)EF.Property<NpgsqlTsVector>(d, "SearchVector")
                .Rank(EF.Functions.WebSearchToTsQuery("english", term));
        }

        // 2. MSSQL / Generic Fallback: Aggregate TF-IDF / Relevance Scores from DocumentKeywords junction table
        return d => d.DocumentKeywords
            .Where(dk => dk.Keyword.Value.Contains(term))
            .Sum(dk => (double)dk.RelevanceScore);
    }
}

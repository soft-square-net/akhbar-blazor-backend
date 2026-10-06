

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ar;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.TextAnalysis.Services;
public class LuceneSearchService
{
    private const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
    private readonly Lucene.Net.Store.Directory _indexDirectory;

    public LuceneSearchService(string indexPath)
    {
        // RAMDirectory for testing, FSDirectory for persistence
        _indexDirectory = FSDirectory.Open(new DirectoryInfo(indexPath));
    }

    private static Analyzer GetAnalyzer(Language language)
    {
        return language switch
        {
            Language.Arabic => new ArabicAnalyzer(AppLuceneVersion),
            _ => new StandardAnalyzer(AppLuceneVersion)
        };
    }

    public void IndexDocument(int id, string title, string content, Language language)
    {
        using var analyzer = GetAnalyzer(language);
        var config = new IndexWriterConfig(AppLuceneVersion, analyzer)
        {
            OpenMode = OpenMode.CREATE_OR_APPEND
        };

        using var writer = new IndexWriter(_indexDirectory, config);

        var doc = new Document
        {
            new StringField("id", id.ToString(), Field.Store.YES),
            new TextField("title", title, Field.Store.YES),
            new TextField("content", content, Field.Store.NO)
        };

        writer.UpdateDocument(new Term("id", id.ToString()), doc);
        writer.Commit();
    }

    public List<int> Search(string searchTerm, Language language, int maxResults = 20)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return new List<int>();

        using var reader = DirectoryReader.Open(_indexDirectory);
        var searcher = new IndexSearcher(reader);
        using var analyzer = GetAnalyzer(language);

        var parser = new MultiFieldQueryParser(
            AppLuceneVersion,
            new[] { "title", "content" },
            analyzer);

        var query = parser.Parse(searchTerm);
        var topDocs = searcher.Search(query, maxResults);

        var resultIds = new List<int>();
        foreach (var scoreDoc in topDocs.ScoreDocs)
        {
            var doc = searcher.Doc(scoreDoc.Doc);
            resultIds.Add(int.Parse(doc.Get("id")));
        }

        return resultIds;
    }
}

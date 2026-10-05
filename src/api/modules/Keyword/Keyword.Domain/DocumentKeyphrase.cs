using System;
using System.Collections.Generic;
using System.Text;

namespace FSH.Starter.WebApi.Keyword.Domain;

public class DocumentKeyphrase
{
    public int DocumentId { get; set; }
    public Document Document { get; set; }

    public int KeyphraseId { get; set; }
    public Keyphrase Keyphrase { get; set; }

    // Text Analysis Metrics
    public int Frequency { get; set; }
    public float RelevanceScore { get; set; }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FSH.Starter.WebApi.Keyword.Domain;

public class Keyphrase
{
    public int Id { get; set; }

    // The phrase (e.g., "artificial intelligence", "New York City")
    public string Value { get; set; }

    // Navigation properties
    public ICollection<DocumentKeyphrase> DocumentKeyphrases { get; set; } = new List<DocumentKeyphrase>();
}

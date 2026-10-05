using System;
using System.Collections.Generic;
using System.Text;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;

namespace FSH.Starter.WebApi.Keyword.Domain;

public class Document: AuditableEntity, IAggregateRoot
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; } // The raw text

    // Navigation properties
    public ICollection<DocumentKeyword> DocumentKeywords { get; set; } = new List<DocumentKeyword>();
    public ICollection<DocumentKeyphrase> DocumentKeyphrases { get; set; } = new List<DocumentKeyphrase>();
}

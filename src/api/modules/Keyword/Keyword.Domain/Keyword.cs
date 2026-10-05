using System;
using System.Collections.Generic;
using System.Text;
using FSH.Framework.Core.Domain;
using FSH.Framework.Core.Domain.Contracts;
using Shared.Enums;

namespace FSH.Starter.WebApi.Keyword.Domain;

public class Keyword : AuditableEntity, IAggregateRoot
{
    public int Id { get; set; }

    // The actual word, stored in lowercase for easier searching
    public string Value { get; set; }

    public PartOfSpeech PartOfSpeech { get; set; }
    public NamedEntityType EntityType { get; set; }

    // Navigation properties
    public ICollection<DocumentKeyword> DocumentKeywords { get; set; } = new List<DocumentKeyword>();
}

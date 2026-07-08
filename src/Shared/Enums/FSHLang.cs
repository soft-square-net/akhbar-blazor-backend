using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums;

public class FSHLang
{
    public static readonly FSHLang arEg = new("ar-EG", "Arabic", rtl: true);
    public static readonly FSHLang enUS = new("en-US", "English(US)");
    // public static readonly FSHLang frFR = new("fr-FR");
    // public static readonly FSHLang duGR = new("du-GR");

    // The backing string value
    public string Value { get; }
    public bool RTL { get; }
    public string DisplayName { get; }

    // Private constructor prevents external instantiation
    private FSHLang(string value, string displayName, bool rtl = false)
    {
        Value = value;
        DisplayName = displayName;
        RTL = rtl;
    }

    // Convert from string to StatusEnum implicitly or via a lookup method
    public static FSHLang FromString(string value)
    {
        var match = AllValues.FirstOrDefault(x => x.Value.Equals(value, StringComparison.OrdinalIgnoreCase));
        return match ?? throw new ArgumentException($"Invalid status: {value}");
    }

    public static IEnumerable<FSHLang> AllValues => new[] { arEg, enUS,/* frFR, duGR*/ };
    public static readonly string[] Values = new[] { arEg.Value, enUS.Value, /*frFR.Value, duGR.Value*/ };
}

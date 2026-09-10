using System;
using System.Reflection;
using System.ComponentModel;

// namespace Shared.Enums;
public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

        if (fieldInfo != null)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
        }

        return value.ToString(); // Fallback to the enum name if no description is found
    }

    public static bool NameEquals<T1, T2>(this T1 enumA, T2 enumB)
            where T1 : struct, Enum
            where T2 : struct, Enum
    {
        return Enum.GetName(typeof(T1), enumA) == Enum.GetName(typeof(T2), enumB);
    }
}




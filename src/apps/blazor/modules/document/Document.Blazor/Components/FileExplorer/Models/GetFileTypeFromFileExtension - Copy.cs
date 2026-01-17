using System.Net.Mime;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Shared.Enums;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Models;
public static class GetFileTypeFromFileExtension
{
    public static FileType GetFileType(this FileModel f)
    {
        foreach (FileType fileType in Enum.GetValues(typeof(FileType)))
        {
            if (fileType.GetDescription().Split(',').Any(ext => string.Equals(ext.Trim().TrimStart('.'), f.Extension.Trim().TrimStart('.'), StringComparison.OrdinalIgnoreCase)))
            {
                return fileType;
            }
        }
        return FileType.Other;
    }

    public static FileType GetFileType(this FolderModel f)
    {
        if (string.IsNullOrWhiteSpace(f.AllowedExtensions))
        {
            return FileType.Other;
        }
        List<FileType> fileTypes = new();
        foreach (var fldrExt in f.AllowedExtensions.Split(','))
        {
            foreach (FileType fileType in Enum.GetValues(typeof(FileType)))
            {
                if (fileType.GetDescription().Split(',').Any(ext => string.Equals(ext.Trim().TrimStart('.'), fldrExt, StringComparison.OrdinalIgnoreCase)))
                {
                    if(fileTypes.Contains(fileType))
                    {
                        continue;
                    }
                    fileTypes.Add(fileType);
                }
            }
        }
        if (fileTypes.Count == 1)
        {
            return fileTypes[0];
        }
        return FileType.Other;
    }
}

//public static class GetMimeTypeFromFileExtension
//{
//    public static string? GetMimeType(this FileModel f)
//    {
//        //var fileType = f.GetFileType();
//        //Type mimeClass = fileType switch
//        //{
//        //    FileType.Audio => typeof(MediaTypeNames.Multipart),
//        //    FileType.Code => typeof(MediaTypeNames.Application),
//        //    FileType.Image => typeof(MediaTypeNames.Image),
//        //    FileType.Document => typeof(MediaTypeNames.Text),
//        //    FileType.Other => typeof(MediaTypeNames.Text),
//        //    FileType.Video => typeof(MediaTypeNames.Multipart)
//        //};

//        // Use reflection to get all nested types (public and non-public, static)
//        string ext = f.Extension.Trim().TrimStart('.').ToLower();
//        string extOuery = ext switch
//        {
//            "txt" => "text/plain",
//            "svg" => "image/svg+xml",
//            "js"  => "text/javascrip",
//            "ts"  => "text/javascrip",
//            "tsx" => "text/javascrip text/html text/css",
//            _ => ""
//        }; 
//        Type[] nestedTypes = typeof(MediaTypeNames).GetNestedTypes(
//            BindingFlags.Static |
//            BindingFlags.Public |
//            BindingFlags.NonPublic);

//        foreach (var g in nestedTypes)
//        {
//            FieldInfo[] mimeTypes = g.GetFields(BindingFlags.Static | BindingFlags.Public);
//            foreach (var mimeType in mimeTypes)
//            {
//                string mime = (string)mimeType.GetValue(null);
//                if (mime.EndsWith(ext))
//                {
//                    return mime;
//                }
//            }
//        }

//        return null;
//    }
//}


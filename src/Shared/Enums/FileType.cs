using System.ComponentModel;

namespace Shared.Enums;

public enum FileType
{
    [Description(".aiff,.mp3,.wav,.aac,.m4a,.flac,.ogg")]
    Audio,
    [Description(".html,.htm,.cshtml,.js,.json,.ts,.tsx,.cs,.php,.css,.scss")]
    Code,
    [Description(".pdf,.doc,.docx,.pptx,.ppt,.xlsx,.xls,.odt,.txt,.rtf,.html,.htm,.csv,.mdb,.accdb")]
    Document,
    [Description(".jpg,.png,.jpeg,.webp")]
    Image,
    [Description(".mp4,.mov,.avi,.wmv,.mkv,.flv,.webm,.mpg,.mpeg")]
    Video,
    [Description("Other File Types")]
    Other
}



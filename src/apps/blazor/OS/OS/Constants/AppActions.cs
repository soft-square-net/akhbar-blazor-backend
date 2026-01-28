using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.OS.Constants;
public static class AppActions
{
    public static class FileManager
    {
        public const string OpenFile = "open-file";
        public const string DownloadFile = "download-file";
        public const string DeleteFile = "delete-file";
        public const string RenameFile = "rename-file";
        public const string CreateFolder = "create-folder";
        public const string UploadFile = "upload-file";
    }
    public static class EditManager { 
        public const string Undo = "undo";
        public const string Redo = "redo";
        public const string Cut = "cut";
        public const string Copy = "copy";
        public const string Paste = "paste";
        public const string SelectAll = "select-all";
        public const string OpenSettings = "open-settings";

    }
    public static class HelpManager { 
        public const string OpenHelp = "open-help";
        public const string AboutApp = "about-app";
    }

    public static class ViewManager { 
        public const string ZoomIn = "zoom-in";
        public const string ZoomOut = "zoom-out";
        public const string ResetZoom = "reset-zoom";
        public const string FullScreen = "full-screen";
    }
    public static class ToolsManager { 
        public const string OpenTerminal = "open-terminal";
        public const string OpenTaskManager = "open-task-manager";
        public const string Options = "options";
    }

}

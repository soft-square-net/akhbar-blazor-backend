

namespace FSH.Starter.Blazor.OS.Interfaces;
public interface IOsWindowsOptions<TDialogOtions>
{
    string ActionsClass { get; set; }
    string ContentClass { get; set; }
    string ContentStyle { get; set; }
    /// <summary>
    /// Adds padding to the sides of this dialog. Defaults to true.
    /// </summary>
    bool Gutters { get; set; }
    string TitleClass { get; set; }
    string Title { get; set; }
    bool Dragable { get; set; }
    bool Visible { get; set; }
    TDialogOtions Options { get; set; }
    string Class { get; set; }
    string Style { get; set; }
    object Tag { get; set; }
    Dictionary<string, object> UserAttributes    { get; set; }

}


public interface IDialogOptions
{
    bool? BackdropClick { get; set; }
    string BackgroundClass { get; set; }
    bool? CloseButton { get; set; }
    bool? CloseOnEscapeKey { get; set; }
    bool? FullScreen { get; set; }
    bool? FullWidth { get; set; }
    MaxWidth? MaxWidth { get; set; }
    bool? NoHeader { get; set; }
    DialogPosition? Position { get; set; }
}

public enum MaxWidth
{
    ExtraExtraLarge,
    ExtraLarge,
    ExtraSmall,
    False,
    Large,
    Medium,
    Small
}

public enum DialogPosition
{
    BottomCenter,
    BottomLeft,
    BottomRight,
    Center,
    CenterLeft,
    CenterRight,
    Custom,
    TopCenter,
    TopLeft,
    TopRight
}

//BackdropClick bool? Allows closing the dialog by clicking outside of the dialog.Defaults to true.
//BackgroundClass string The custom CSS classes to apply to the dialog background.Multiple classes must be separated by spaces.
//CloseButton bool? Shows a close button in the top-right corner of the dialog.Defaults to false.
//CloseOnEscapeKey bool? Allows closing the dialog by pressing the Escape key.
//FullScreen  bool? Sets the size of the dialog to the entire screen.Defaults to false.
//FullWidth   bool? Sets the width of the dialog to the width of the screen.Defaults to false.
//MaxWidth MaxWidth?	The maximum allowed width of the dialog.Defaults to null.
//NoHeader    bool? Hides the dialog header.Defaults to false.
//Position DialogPosition?	

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using MudBlazor.State.Builder;

// namespace FSH.Starter.BlazorShared.Components.Dialogs;
#nullable enable
namespace MudBlazor
{
    public partial class MudDialogResizable : MudDialog
    {
        [CascadingParameter] IMudDialogInstance Instance { get; set; } = default!;
        // private string _dialogId;
        private DialogOptions _globalDialogOptions = new();
        private RegisterParameterBuilder<bool> _visibleState;

        [Inject] IJSRuntime JSRuntime { get; set; } = default!;
        // [Inject] MudDialogProvider DialogProvider { get; set; } = default!;



        private DialogOptions GlobalDialogOptions { get; init; } = new();

        // [Parameter] public DefaultFocus? DefaultFocus { get; set; } = MudBlazor.DefaultFocus.Element;
        [Parameter] public bool IsInline { get; set; }

        public MudDialogResizable() : base()
        {

            // _dialogId = $"dialogResizable-{Guid.NewGuid():N}";
            // UserAttributes.Add("id", _dialogId);
            //GlobalDialogOptions = new()
            //{ 
            //    BackdropClick = DialogProvider.BackdropClick,
            //    CloseOnEscapeKey = DialogProvider.CloseOnEscapeKey,
            //    CloseButton = DialogProvider.CloseButton,
            //    NoHeader = DialogProvider.NoHeader,
            //    Position = DialogProvider.Position,
            //    FullWidth = DialogProvider.FullWidth,
            //    MaxWidth = DialogProvider.MaxWidth,
            //    BackgroundClass = DialogProvider.BackgroundClass
            //};
#pragma warning disable CS0618 // Type or member is obsolete
            var parameters = new DialogParameters
            {
                [nameof(Class)] = Class,
                [nameof(Style)] = Style,
                [nameof(Tag)] = Tag,
                [nameof(UserAttributes)] = UserAttributes,
                [nameof(TitleContent)] = TitleContent,
                [nameof(DialogContent)] = DialogContent,
                [nameof(DialogActions)] = DialogActions,
                [nameof(OnBackdropClick)] = OnBackdropClick,
                [nameof(Gutters)] = Gutters,
                [nameof(TitleClass)] = TitleClass,
                [nameof(ContentClass)] = ContentClass,
                [nameof(ActionsClass)] = ActionsClass,
                [nameof(ContentStyle)] = ContentStyle,
                [nameof(DefaultFocus)] = DefaultFocus,
            };
#pragma warning restore CS0618 // Type or member is obsolete
            _globalDialogOptions = GlobalDialogOptions;
        }

        override protected void OnParametersSet()
        {
            base.OnParametersSet();

        }
        override protected void OnInitialized()
        {
            base.OnInitialized();

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                MakeMovableAsync(Instance.Id.ToString());
                // this.DialogService.DialogInstanceAddedAsync += OnDialogInstanceAddedAsync;

            }
        }


        //private async Task OnDialogInstanceAddedAsync(IDialogReference reference)
        //{

        //    if (!string.IsNullOrWhiteSpace(reference.Id.ToString()))
        //    {
        //        //if (UserAttributes.TryGetValue("id", out object _dialogId) || !string.IsNullOrWhiteSpace(FieldId))MakeMovableAsync
        //        //{

        //        MakeMovableAsync(reference.Id.ToString());
        //        // }
        //    }
        //}

        private async Task MakeMovableAsync(string Id)
        {
            await JSRuntime.InvokeVoidAsync("mudBlazorDialogHelper.init", $"_{Id.Replace("-", "")}");
        }
        // You can add your own methods and properties here
        void Save()
        {
            // Logic to save data
            this.CloseAsync(DialogResult.Ok(true)); // Close with a result
        }
    }
}

using AdminDashboard.Services.Interfaces;
using BlazorModularEngine.Core.Common.Plugins;
using Microsoft.AspNetCore.Components.Rendering;

namespace AdminDashboard.Components;

public class AdminDashboardComponent : PluginComponentBase
{
    private IAdminStatsService? _statsService;
    private int _users = 0;
    private decimal _revenue = 0;
    private bool _isLoading = true;

    protected override async Task OnInitializedAsync()
    {
        _statsService = GetService<IAdminStatsService>();
        if (_statsService != null)
        {
            _users = await _statsService.GetActiveUsersCountAsync();
            _revenue = await _statsService.GetTotalRevenueAsync();
        }
        _isLoading = false;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        int seq = 0;
        builder.OpenElement(seq++, "div");
        builder.AddAttribute(seq++, "class", "p-6 bg-white rounded-xl shadow-md border border-slate-100 space-y-6");
        {
            // Header
            builder.OpenElement(seq++, "div");
            builder.AddAttribute(seq++, "class", "flex justify-between items-center border-b pb-4");
            {
                builder.OpenElement(seq++, "h2");
                builder.AddAttribute(seq++, "class", "text-2xl font-bold text-slate-800");
                builder.AddContent(seq++, "Admin Dashboard Plugin");
                builder.CloseElement();

                builder.OpenElement(seq++, "span");
                builder.AddAttribute(seq++, "class", "px-3 py-1 bg-indigo-50 text-indigo-600 rounded-full text-xs font-semibold");
                builder.AddContent(seq++, "v1.0.0 Active");
                builder.CloseElement();
            }
            builder.CloseElement();

            if (_isLoading)
            {
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "py-8 text-center text-slate-500 font-medium");
                builder.AddContent(seq++, "Loading admin statistics...");
                builder.CloseElement();
            }
            else
            {
                // Stats Grid
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "class", "grid grid-cols-1 md:grid-cols-2 gap-6");
                {
                    // Card 1
                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "class", "p-5 bg-slate-50 rounded-lg border border-slate-200");
                    {
                        builder.OpenElement(seq++, "p");
                        builder.AddAttribute(seq++, "class", "text-sm text-slate-500 font-medium");
                        builder.AddContent(seq++, "Active Users");
                        builder.CloseElement();

                        builder.OpenElement(seq++, "h3");
                        builder.AddAttribute(seq++, "class", "text-3xl font-extrabold text-slate-900 mt-1");
                        builder.AddContent(seq++, _users.ToString("N0"));
                        builder.CloseElement();
                    }
                    builder.CloseElement();

                    // Card 2
                    builder.OpenElement(seq++, "div");
                    builder.AddAttribute(seq++, "class", "p-5 bg-slate-50 rounded-lg border border-slate-200");
                    {
                        builder.OpenElement(seq++, "p");
                        builder.AddAttribute(seq++, "class", "text-sm text-slate-500 font-medium");
                        builder.AddContent(seq++, "Total Revenue");
                        builder.CloseElement();

                        builder.OpenElement(seq++, "h3");
                        builder.AddAttribute(seq++, "class", "text-3xl font-extrabold text-emerald-600 mt-1");
                        builder.AddContent(seq++, $"${_revenue:N2}");
                        builder.CloseElement();
                    }
                    builder.CloseElement();
                }
                builder.CloseElement();
            }
        }
        builder.CloseElement();
    }
}

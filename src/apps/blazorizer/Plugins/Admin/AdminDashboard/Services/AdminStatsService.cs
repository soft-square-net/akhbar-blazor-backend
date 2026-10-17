using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminDashboard.Services.Interfaces;

namespace AdminDashboard.Services;

public class AdminStatsService : IAdminStatsService
{
    public async Task<int> GetActiveUsersCountAsync()
    {
        await Task.Delay(100);
        return 1420;
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        await Task.Delay(100);
        return 48250.00m;
    }
}

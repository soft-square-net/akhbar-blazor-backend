using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminDashboard.Services.Interfaces;

public interface IAdminStatsService
{
    Task<int> GetActiveUsersCountAsync();
    Task<decimal> GetTotalRevenueAsync();
}

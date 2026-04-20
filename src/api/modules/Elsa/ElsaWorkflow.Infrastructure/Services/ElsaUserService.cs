using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Elsa.Identity.Entities;
using FSH.Framework.Core.Identity.Users.Abstractions;
using FSH.Framework.Core.Identity.Users.Dtos;
using FSH.Framework.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FSH.Starter.ElsaWorkflow.Infrastructure.Services;

public interface IElsaUserService
{
    Task<FshUser?> ValidateAndGetUserAsync(string authHeader);
    Task<List<UserRoleDetail>> GetUserRolesAsync(FshUser user, CancellationToken cancellationToken);
}

// Example implementation
public class CustomUserService : IElsaUserService
{
    private readonly ILogger<CustomUserService> _logger;
    private readonly HttpClient _httpClient;
    private readonly IUserService _userService;
    private readonly SignInManager<FshUser> _signInManager;

    public CustomUserService(ILogger<CustomUserService> logger, HttpClient httpClient, IUserService userService, SignInManager<FshUser> signInManager)
    {
        _logger = logger;
        _httpClient = httpClient;
        _userService = userService;
        _signInManager = signInManager;
    }

    public async Task<List<UserRoleDetail>> GetUserRolesAsync(FshUser user, CancellationToken cancellationToken)
    { 
        return await _userService.GetUserRolesAsync(user.Id, cancellationToken);
    }
    public async Task<FshUser?> ValidateAndGetUserAsync(string authHeader)
    {
        try
        {
            // Example: Call external authentication service
            var response = await _httpClient.GetAsync($"https://auth-service.com/validate?token={authHeader}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var user = await response.Content.ReadFromJsonAsync<FshUser>();
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user");
            return null;
        }
    }
}

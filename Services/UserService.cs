using Microsoft.AspNetCore.Http.HttpResults;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Data;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.DTOs;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Services;

public class UserService(AppDbContext dbContext)
{
    public async Task<User?> GetCurrentUserAsync(int userId)
    {
        var res = await dbContext.Users.FindAsync(userId);
        return res;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<User?> UpdateUserAsync(UserDto patch, int userId)
    {
        var user = await dbContext.Users.FindAsync(userId);
        if (user == null) return null;
        
        if (patch.Name != user.Name && patch.Name != null) user.Name = patch.Name;
        if (patch.AccessLevel != user.AccessLevel && patch.AccessLevel != null) user.AccessLevel = patch.AccessLevel.Value;
        
        await dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await dbContext.Users.FindAsync(userId);
        if (user == null) return false;

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();

        return true;
    }
}
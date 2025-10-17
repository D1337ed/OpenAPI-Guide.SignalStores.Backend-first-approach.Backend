using Microsoft.AspNetCore.Http.HttpResults;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Data;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Services;

public class UserService(UserDbContext userDbContext)
{
    public async Task<Ok<User>> GetCurrentUserAsync(int userId)
    {
        var res = await userDbContext.Users.FindAsync(userId);
        return TypedResults.Ok(res);
    }

    public async Task<IResult> CreateUserAsync(User user)
    {
        try
        {
            await userDbContext.Users.AddAsync(user);
            await userDbContext.SaveChangesAsync();

            return Results.Created("/users", user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> UpdateUserAsync(User patch, int userId)
    {
        try
        {
            var user = await userDbContext.Users.FindAsync(userId);
            
            if (user == null) return Results.NotFound("User not found");
            user.Name = patch.Name;
            user.AccessLevel = patch.AccessLevel;
            
            await userDbContext.SaveChangesAsync();

            return Results.Json(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> DeleteUserAsync(int userId)
    {
        try
        {
            var user = await userDbContext.Users.FindAsync(userId);
            
            if (user == null) return Results.NotFound("User not found");

            userDbContext.Users.Remove(user);
            await userDbContext.SaveChangesAsync();

            return Results.NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }
}
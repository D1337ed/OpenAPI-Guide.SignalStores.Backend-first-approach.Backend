using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Data;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.DTOs;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        policy.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod(); // docker nginx
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors();
app.UseHttpsRedirection();

app.MapGet("/", () => "visit /movies or /users");

app.MapGet("/movies", async (MovieService movieService) =>
{
    try
    {
        var movies = await movieService.GetAllMoviesAsync();
        return Results.Ok(movies);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});
app.MapGet("/movies/{movieId:int}", async (MovieService movieService, int movieId) =>
{
    try
    {
        var movie = await movieService.GetMovieByIdAsync(movieId);
        return movie == null ? Results.NotFound("Movie not found") : Results.Ok(movie);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});
app.MapPost("/movies", async (MovieService movieService, Movie movie) =>
{
    try
    {
        var res = await movieService.CreateMovieAsync(movie);
        return Results.Created($"/movies/{res.Id}", res);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});
app.MapPatch("/movies/{movieId:int}", async (MovieService movieService, MovieDto patch, int movieId) =>
{
    try
    {
        var res = await movieService.UpdateMovieAsync(patch, movieId);
        return res == null ? Results.NotFound("Movie not found, could not patch") : Results.Ok(res);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});
app.MapDelete("/movies/{movieId:int}", async (MovieService movieService, int movieId) =>
{
    try
    {
        var res = await movieService.DeleteMovieAsync(movieId);
        return res ? Results.NoContent() : Results.NotFound("Movie not found, could not delete");
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});

app.MapGet("/users/{userId:int}", async (UserService userService, int userId) =>
{
    try
    {
        var user = await userService.GetCurrentUserAsync(userId);
        return user == null ? Results.NotFound("User not found") : Results.Ok(user);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});
app.MapPost("/users", async (UserService userService, User user) =>
{
    try
    {
        var res = await userService.CreateUserAsync(user);
        return Results.Created($"users/{res.Id}", res);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});
app.MapPatch("/users/{userId:int}", async (UserService userService, UserDto patch, int userId) =>
{
    try
    {
        var res = await userService.UpdateUserAsync(patch, userId);
        return res == null ? Results.NotFound("User not found, could not patch") : Results.Ok(res);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});
app.MapDelete("/users/{userId:int}", async (UserService userService, int userId) =>
{
    try
    {
        var res = await userService.DeleteUserAsync(userId);
        return res ? Results.NoContent() : Results.NotFound("User not found, could not delete");
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.Problem(e.Message);
    }
});

app.Run();
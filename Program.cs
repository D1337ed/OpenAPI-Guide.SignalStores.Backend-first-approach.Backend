using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Data;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<MovieDbContext>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddDbContext<UserDbContext>();
builder.Services.AddScoped<UserService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
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

app.MapGet("/movies", (MovieService movieService) =>
{
    var movies = movieService.GetAllMoviesAsync();
    return movies;
});
app.MapGet("/movies/{movieId:int}", (MovieService movieService, int movieId) =>
{
    var movie = movieService.GetMovieByIdAsync(movieId);
    return movie;
});
app.MapPost("/movies", (MovieService movieService, Movie movie) =>
{
    var res = movieService.CreateMovieAsync(movie);
    return res;
});
app.MapPatch("/movies/{movieId:int}", (MovieService movieService, Movie patch, int movieId) =>
{
    var res = movieService.UpdateMovieAsync(patch, movieId);
    return res;
});
app.MapDelete("/movies/{movieId:int}", (MovieService movieService, int movieId) =>
{
    var res = movieService.DeleteMovieAsync(movieId);
    return res;
});

app.MapGet("/users/{userId:int}", (UserService userService, int userId) =>
{
    var user = userService.GetCurrentUserAsync(userId);
    return user;
});
app.MapPost("/users", (UserService userService, User user) =>
{
    var res = userService.CreateUserAsync(user);
    return res;
});
app.MapPatch("/users/{userId:int}", (UserService userService, User patch, int userId) =>
{
    var res = userService.UpdateUserAsync(patch, userId);
    return res;
});
app.MapDelete("/users/{userId:int}", (UserService userService, int userId) =>
{
    var res = userService.DeleteUserAsync(userId);
    return res;
});

app.Run();
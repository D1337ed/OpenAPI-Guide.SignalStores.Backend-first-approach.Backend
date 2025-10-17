using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Data;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Services;

public class MovieService(MovieDbContext movieDbContext)
{
    public async Task<Ok<Movie[]>> GetAllMoviesAsync()
    {
        var res = await movieDbContext.Movies.ToArrayAsync();
        return TypedResults.Ok(res);
    }

    public async Task<IResult> GetMovieByIdAsync(int movieId)
    {
        try
        {
            var movie = await movieDbContext.Movies.FindAsync(movieId);
            return movie == null ? Results.NotFound("Movie not found") : Results.Ok(movie);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> CreateMovieAsync(Movie movie)
    {
        try
        {
            await movieDbContext.Movies.AddAsync(movie);
            await movieDbContext.SaveChangesAsync();
            
            return Results.Created("/movies", movie);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> UpdateMovieAsync(Movie patch, int movieId)
    {
        try
        {
            var movie = await movieDbContext.Movies.FindAsync(movieId);
            
            if (movie == null) return Results.NotFound("Movie not found");
            movie.Title = patch.Title;
            movie.Studio = patch.Studio;
            movie.Year = patch.Year;

            await movieDbContext.SaveChangesAsync();

            return Results.Json(movie);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }

    public async Task<IResult> DeleteMovieAsync(int movieId)
    {
        try
        {
            var movie = await movieDbContext.Movies.FindAsync(movieId);

            if (movie == null) return Results.NotFound("Movie not found");

            movieDbContext.Movies.Remove(movie);
            await movieDbContext.SaveChangesAsync();
            
            return Results.NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.BadRequest(e.Message);
        }
    }
}
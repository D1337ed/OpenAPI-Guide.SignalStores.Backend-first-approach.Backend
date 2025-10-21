using Microsoft.EntityFrameworkCore;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Data;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.DTOs;
using OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Services;

public class MovieService(AppDbContext dbContext)
{

    public async Task<Movie[]> GetAllMoviesAsync()
    {
        var movies = await dbContext.Movies.ToArrayAsync();
        return movies;
    }

    public async Task<Movie?> GetMovieByIdAsync(int movieId)
    {
        var movie = await dbContext.Movies.FindAsync(movieId);
        return movie ?? null;
    }

    public async Task<Movie> CreateMovieAsync(Movie movie)
    {
            await dbContext.Movies.AddAsync(movie);
            await dbContext.SaveChangesAsync();
            
            return movie;
    }

    public async Task<Movie?> UpdateMovieAsync(MovieDto patch, int movieId)
    {

        var movie = await dbContext.Movies.FindAsync(movieId);
        if (movie == null) return null;

        if (patch.Title != movie.Title && patch.Title != null) movie.Title = patch.Title;
        if (patch.Studio != movie.Studio && patch.Studio != null) movie.Studio = patch.Studio;
        if (patch.Year != movie.Year && patch.Year != null) movie.Year = patch.Year.Value;

        await dbContext.SaveChangesAsync();

        return movie;
    }

    public async Task<bool> DeleteMovieAsync(int movieId)
    {
        var movie = await dbContext.Movies.FindAsync(movieId);
        if (movie == null) return false;

        dbContext.Movies.Remove(movie);
        await dbContext.SaveChangesAsync();
        
        return true;
    }
}
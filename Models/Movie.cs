namespace OpenAPI_Guide.SignalStores.Backend_first_approach.Backend.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Studio { get; set; }
    public required int Year { get; set; }
}
using LibraryManagement.Api.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LibraryManagement.Api.Endpoints;

public static class CategoriesEndpoints
{
    private static List<Category> _categories =
    [
        new Category {Id = 1, Name = "Biography"},
        new Category {Id = 2, Name = "Novel"}
    ];

    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/api/categories")
            .WithTags("Categories");

        group.MapGet("/{id:int}", GetById);
        // .Produces<Category>(StatusCodes.Status200OK)
        // .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/", GetAll);

        group.MapPost("/", Create);

    }

    private static async Task<Results<Ok<Category>, NotFound>> GetById(int id)
    {
        var category = _categories.FirstOrDefault(
            category => category.Id == id);

        if (category is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(category);
    }

    private static async Task<Ok<List<Category>>> GetAll()
    {
        var categories = _categories;
        return TypedResults.Ok(categories);
    }

    private static async Task<Results<Created<Category>, Conflict, BadRequest>> Create(
        Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            return TypedResults.BadRequest();
        }

        if (_categories.Any(existingCategory => existingCategory.Id == category.Id))
        {
            return TypedResults.Conflict();
        }

        if (_categories.Any(existingCategory => existingCategory.Name == category.Name))
        {
            return TypedResults.Conflict();
        }

        _categories.Add(category);
    }
}

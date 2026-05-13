namespace WebApp.Client.Infrastructure.Contracts.Categories;

public sealed class CategoriesResponseModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int CategoryTypeId { get; init; }
    public string CategoryType { get; init; } = string.Empty;
}


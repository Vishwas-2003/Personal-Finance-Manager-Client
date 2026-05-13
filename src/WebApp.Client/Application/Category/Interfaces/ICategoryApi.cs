namespace WebApp.Client.Application.Category.Interfaces;

public interface ICategoryApi
{
    Task<IReadOnlyList<CategoryItem>> GetCategoriesAsync(int userId, CancellationToken cancellationToken);
}


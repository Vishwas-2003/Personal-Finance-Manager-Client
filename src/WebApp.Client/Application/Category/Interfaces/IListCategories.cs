namespace WebApp.Client.Application.Category.Interfaces;

public interface IListCategories
{
    Task<IReadOnlyList<CategoryItem>> ExecuteAsync(CancellationToken cancellationToken);
}


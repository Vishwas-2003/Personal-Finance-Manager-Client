using WebApp.Client.Application.Category;
using WebApp.Client.Application.Category.Interfaces;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Session.Interfaces;

namespace WebApp.Client.Application.Expenses;

public sealed class ListCategory(ICategoryApi categoryApi, ISessionAccessor sessionAccessor) : IListCategories
{
    public async Task<IReadOnlyList<CategoryItem>> ExecuteAsync(CancellationToken cancellationToken)
    {
        var session = sessionAccessor.Current ?? throw new InvalidOperationException(AppConstants.Messages.MustLoginFirst);
        return await categoryApi.GetCategoriesAsync(session.UserId, cancellationToken);
    }
}


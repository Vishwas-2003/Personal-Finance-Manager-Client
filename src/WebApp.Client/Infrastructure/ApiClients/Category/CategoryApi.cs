using WebApp.Client.Application.Category;
using WebApp.Client.Application.Category.Interfaces;
using WebApp.Client.Application.Expenses;
using WebApp.Client.Constants;
using WebApp.Client.Infrastructure.Configuration;
using WebApp.Client.Infrastructure.Contracts.Categories;
using WebApp.Client.Infrastructure.Http;
using WebApp.Client.Infrastructure.Routing;

namespace WebApp.Client.Infrastructure.ApiClients.Category;

public sealed class CategoryApi(IHttpClientFactory httpClientFactory) : ICategoryApi
{

    public async Task<IReadOnlyList<CategoryItem>> GetCategoriesAsync(int userId, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient(ApiHttpClientNames.Authenticated);
        var path = RouteConstants.Category.GetCategories;
        var response = await client.GetAsync(path, cancellationToken);
        var payload = await JsonHttp.ReadOrThrowAsync<List<CategoriesResponseModel>>(response, cancellationToken);
        return payload.Select(ToItem).ToArray();
    }

    private static CategoryItem ToItem(CategoriesResponseModel r) =>
        new(
            r.Id,
            r.Name,
            r.CategoryType);
}


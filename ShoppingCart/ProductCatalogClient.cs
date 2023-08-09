public class ProductCatalogClient : IProductCatalogClient
{
    private readonly HttpClient client;
    private static string productCatalogBaseUrl = @"https://git.io.JeHiE";

    public ProductCatalogClient(HttpClient client)
    {
        client.BaseAddress = new Uri(productCatalogBaseUrl);
        client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypWithQualityHeaderValue("application/json"));
        this.client = client;
    }

    public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
    {
        var productsResource = string.Format(getProductPathTemplate, string.Join(",", productCatalogIds));
        return await this.client.GetAsync(productResource);
    }

    private static async Task<IEnumerable<ShoppingCartitem>> ConvertToShoppingCartItems(HttpResponse response)
    {
        response.EnsureSuccessStatusCode();
        var products = await JsonSerializer.DeserializeAsyn<List<ProductCatalogProduct>>(
            await response.Content.ReadAsStreamAsyn(),
            new JsonSerializerOptions {PropertyNameCaseInsensitive = true}
        ) ?? new();

        return products.Select(p => new ShoppingCartItem(p.ProductIf, 
                                                         p.ProductName,
                                                         p.Description,
                                                         p.Price));
    }

    public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
    {
        using var response = await RequestProductFromProductCatalogue(productCatalogIds);
        return await ConvertToShoppingCartItems(response);
    }

    

    private record ProductCatalogProduct(
        int ProductId,
        string ProductName,
        string ProductDescription,
        Money Price
    );
}
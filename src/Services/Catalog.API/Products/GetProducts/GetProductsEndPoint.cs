namespace Catalog.API.Products.GetProducts
{
    public record GetProductsResponse (IEnumerable<Product> Products);

    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender, CancellationToken cancellationToken) =>
            {
                var request = new GetProductsQuery();
                var result = await sender.Send(request, cancellationToken);
                var response = result.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .WithTags("Products");
        }
    }
}

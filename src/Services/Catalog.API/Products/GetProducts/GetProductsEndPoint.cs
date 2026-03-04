namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
    public record GetProductsResponse (IEnumerable<Product> Products);

    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductsRequest request,  ISender sender, CancellationToken cancellationToken) =>
            {
                var query = request.Adapt<GetProductsQuery>();
                var result = await sender.Send(query, cancellationToken);
                var response = result.Adapt<GetProductsResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .WithTags("Products");
        }
    }
}

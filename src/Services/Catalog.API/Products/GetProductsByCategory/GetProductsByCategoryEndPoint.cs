
namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductsByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var result = await sender.Send(new GetProductsByCategoryQuery(category));
                var response = result.Adapt<GetProductByCategoryResponse>();
                return Results.Ok(response);
            })
            .WithTags("Products")
            .WithName("GetProductsByCategory")
            .WithSummary("Get products by category")
            .WithDescription("Retrieves a list of products that belong to the specified category.");
        }
    }
}

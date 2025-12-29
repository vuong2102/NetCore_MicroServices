
namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdResponse(Product Product);

    public class GetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByIdQuery(id));
                var response = result.Adapt<GetProductByIdResponse>();
                return Results.Ok(response);
            })
            .WithName("GetProductById")
            .WithTags("Products")
            .WithSummary("Get Product By Id");
        }
    }
}

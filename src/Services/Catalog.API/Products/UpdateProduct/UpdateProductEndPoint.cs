namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile);
    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await sender.Send(command, cancellationToken);
                var response = result.Adapt<UpdateProductResponse>();
                return Results.Ok(response);
            })
            .WithName("UpdateProduct")
            .WithTags("Products")
            .WithSummary("Update an existing product")
            .WithDescription("Updates the details of an existing product in the catalog.");
        }
    }
}

namespace Basket.API.Basket.GetBasket
{
    public record GetBasketResponse(ShoppingCart Basket);

    public class GetBasketEndPoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/Basket/{userName}", async (string userName, ISender sender, CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName));
                var response = result.Adapt<GetBasketResult, GetBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("GetBasket")
            .WithTags("Basket")
            .Produces<ShoppingCart>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}

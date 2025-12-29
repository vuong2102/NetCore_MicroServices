
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, List<string> Category, string ImageFile) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);
    public class UpdateProductHandler
        (IDocumentSession session, ILogger<UpdateProductHandler> logger)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product == null)
            {
                logger.LogWarning("Product with Id {ProductId} not found.", command.Id);
                return new UpdateProductResult(false);
            }
            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.Category = command.Category;

            session.Update(product);
            await session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(true);
        }
    }
}

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand
    (
        string Name,
        List<string> Category,
        string Description,
        decimal Price,
        string ImageFile
    ) : ICommand<CreateProductResult>;

    public record CreateProductResult
    (
        Guid Id
    );

    internal class CreateProductCommandHandler(IDocumentSession session) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                Price = command.Price,
                ImageFile = command.ImageFile
            };

            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            return new CreateProductResult(product.Id);
        }
    }
}

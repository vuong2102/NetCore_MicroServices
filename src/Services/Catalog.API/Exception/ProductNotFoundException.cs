namespace Catalog.API.Exception
{
    public class ProductNotFoundException : System.Exception
    {
        public ProductNotFoundException(string message) : base("Product not found!")
        {
        }
    }
}

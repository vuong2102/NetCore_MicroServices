namespace Discount.Grpc.Models
{
    public class Coupon
    {
        public required string Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public float Amount { get; set; }
    }
}

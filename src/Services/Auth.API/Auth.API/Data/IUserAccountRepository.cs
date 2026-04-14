using Auth.Grpc.Models.Models;

namespace Auth.Grpc.Data
{
    public interface IUserAccountRepository
    {
        Task<UserAccount> GetBasket(string userName, CancellationToken cancellationToken = default);
        Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default);
        Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default);
    }
}

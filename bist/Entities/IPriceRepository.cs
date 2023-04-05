using bist.Models;

namespace bist.Entities
{
    public interface IPriceRepository
    {
        IQueryable<Price> Prices { get; }
        Price GetById(int PriceId);
        IEnumerable<Price> GetPrices();
        void CreatePrice(Price newPrice);
        void UpdatePrice(Price updatedPrice);
        void DeletePrice(int PriceId);
    }
}

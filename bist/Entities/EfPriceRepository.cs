using bist.Models;

namespace bist.Entities
{
    public class EfPriceRepository : IPriceRepository
    {
        private readonly DatabaseContext context;

        public EfPriceRepository(DatabaseContext _context)
        {
            context = _context;
        }
        public IEnumerable<Price> Prices => context.prices;
        IQueryable<Price> IPriceRepository.Prices => context.prices;

        public void CreatePrice(Price newPrice)
        {
            context.prices.Add(newPrice);
            context.SaveChanges();
        }

        public void DeletePrice(int PriceId)
        {
            var entity = context.prices.Find(PriceId);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The hisse entity cannot be null.");
            }
            else
            {
                context.prices.Remove(entity);
                context.SaveChanges();
            }
        }

        public Price GetById(int PriceId)
        {
            var entity = context.prices.FirstOrDefault(a => a.Id == PriceId);
            if (entity == null)
            {
                throw new ArgumentException("The admin entity with the specified id could not be found.", nameof(PriceId));
            }

            return entity;
        }

        public IEnumerable<Price> GetPrices()
        {
            return context.prices.ToList();
        }

        public void UpdatePrice(Price updatedPrice)
        {
            context.prices.Update(updatedPrice);
            context.SaveChanges();
        }
    }
}

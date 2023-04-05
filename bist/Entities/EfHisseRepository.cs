using bist.Models;

namespace bist.Entities
{
    public class EfHisseRepository : IHisseRepository
    {
        private DatabaseContext context;
        
        public EfHisseRepository(DatabaseContext _context)
        {
            context = _context;
        }

        public IEnumerable<Hisse> Hisses => context.hisses;
        IQueryable<Hisse> IHisseRepository.Hisses => context.hisses;

        public void CreateHisse(Hisse newHisse)
        {
            context.hisses.Add(newHisse);
            context.SaveChanges();
        }

        public void DeleteHisse(int HisseId)
        {
            var entity = context.hisses.Find(HisseId);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The hisse entity cannot be null.");
            }
            else
            {
                context.hisses.Remove(entity);
                context.SaveChanges();
            }
        }

        public Hisse GetById(int HisseId)
        {
            var entity = context.hisses.FirstOrDefault(a => a.Id == HisseId);
            if (entity == null)
            {
                throw new ArgumentException("The admin entity with the specified id could not be found.", nameof(HisseId));
            }

            return entity;
        }

        public IEnumerable<Hisse> GetHisses()
        {
            return context.hisses.ToList();
        }

        public void UpdateHisse(Hisse updatedHisse)
        {
            context.hisses.Update(updatedHisse);
            context.SaveChanges();
        }
    }
}

using bist.Models;

namespace bist.Entities
{
    public interface IHisseRepository
    {
        IQueryable<Hisse> Hisses { get; }
        Hisse GetById(int HisseId);
        IEnumerable<Hisse> GetHisses();
        void CreateHisse(Hisse newHisse);
        void UpdateHisse(Hisse updatedHisse);
        void DeleteHisse(int HisseId);
    }
}

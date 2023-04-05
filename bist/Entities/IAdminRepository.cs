using bist.Models;

namespace bist.Entities
{
    public interface IAdminRepository
    {
        IQueryable<Admin> Admins { get; }
        Admin GetById(int AdminId);
        IEnumerable<Admin> GetAdmins();
        void CreateAdmin(Admin newAdmin);
        void UpdateAdmin(Admin updatedAdmin);
        void DeleteAdmin(int AdminId);
    }
}

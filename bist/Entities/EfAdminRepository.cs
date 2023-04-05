using bist.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace bist.Entities
{
    public class EfAdminRepository : IAdminRepository
    {
        private  DatabaseContext context;
        public EfAdminRepository(DatabaseContext _context)
        {
            context = _context;
        }

        public IEnumerable<Admin> Admins => context.admins;
        IQueryable<Admin> IAdminRepository.Admins => context.admins;

        public void CreateAdmin(Admin newAdmin)
        {
            context.admins.Add(newAdmin);
            context.SaveChanges();
        }

        public void DeleteAdmin(int AdminId)
        {
            var entity = context.admins.Find(AdminId);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The admin entity cannot be null.");
            }
            else
            {
                context.admins.Remove(entity);
                context.SaveChanges();
            }
            
        }

        public IEnumerable<Admin> GetAdmins()
        {
            return context.admins.ToList();
        }

        public Admin GetById(int AdminId)
        {
            var entity = context.admins.FirstOrDefault(a => a.Id == AdminId);
            if (entity == null)
            {
                throw new ArgumentException("The admin entity with the specified id could not be found.", nameof(AdminId));
            }

            return entity;          
        }

        public void UpdateAdmin(Admin updatedAdmin)
        {
            context.admins.Update(updatedAdmin);
            context.SaveChanges();
        }
    }
}

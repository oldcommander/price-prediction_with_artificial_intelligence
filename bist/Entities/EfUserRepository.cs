using bist.Models;

namespace bist.Entities
{
    public class EfUserRepository : IUserRepository
    {
        private DatabaseContext context;
        public EfUserRepository(DatabaseContext _context)
        {
            context = _context;
        }

        public IEnumerable<User> Users => context.users;
        IQueryable<User> IUserRepository.Users => context.users;

        public void CreateUser(User newUser)
        {
            context.users.Add(newUser);
            context.SaveChanges();
        }

        public void DeleteUser(int UserId)
        {
            var entity = context.users.Find(UserId);
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "The admin entity cannot be null.");
            }
            else
            {
                context.users.Remove(entity);
                context.SaveChanges();
            }

        }

        public IEnumerable<User> GetUsers()
        {
            return context.users.ToList();
        }

        public User GetById(int UserId)
        {
            var entity = context.users.FirstOrDefault(a => a.Id == UserId);
            if (entity == null)
            {
                throw new ArgumentException("The admin entity with the specified id could not be found.", nameof(UserId));
            }

            return entity;
        }

        public void UpdateUser(User updatedUser)
        {
            context.users.Update(updatedUser);
            context.SaveChanges();
        }
    }
}

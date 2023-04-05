using bist.Models;

namespace bist.Entities
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        User GetById(int UserId);
        IEnumerable<User> GetUsers();
        void CreateUser(User newUser);
        void UpdateUser(User updatedUser);
        void DeleteUser(int UserId);
    }
}

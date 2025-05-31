using BlogApp.Entities;

namespace BlogApp.DataLayer.Abstract
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        void CreateUser(User user);
        User ValidateUser(string email, string password);

        Task<bool> UpdateUserImage(int userId, string imageName);
        void UpdateUser(User user);
        Task<bool> SaveChangesAsync();
    }
}

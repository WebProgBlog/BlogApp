using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Abstract
{
    public interface IUserService
    {
        IQueryable<User> GetAllUsers();
        void CreateUser(User user);
        User ValidateUser(string email, string password);
        Task<bool> UpdateUserImage(int userId, string imageName);
        void UpdateUser(User user);
        User GetUserById(int userId);
        void DeleteUser(User user);
        Task<bool> SaveChangesAsync();
    }
}

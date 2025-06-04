using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Abstract
{
    public interface IUserService
    {
        User? ValidateUser(string username, string password);
        IQueryable<User> GetAllUsers();
        void CreateUser(User user);
        Task<bool> UpdateUserImage(int userId, string imageName);
        void UpdateUser(User user);
        User GetUserById(int userId);
        void DeleteUser(User user);
        Task<bool> SaveChangesAsync();
    }
}

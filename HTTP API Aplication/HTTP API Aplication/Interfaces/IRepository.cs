using HTTP_API_Aplication.DTO;
using HTTP_API_Aplication.Models;

namespace HTTP_API_Aplication.Interfaces
{
    public interface IRepository
    {
        List<User> GetAllUsers();
        void AddUser(User newUser);
        void UpdateUser(User updatedUser);
        void DeleteUser(User userToDelete);
    }
}

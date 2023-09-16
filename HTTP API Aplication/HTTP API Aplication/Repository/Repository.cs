using HTTP_API_Aplication.Models;
using System.Web;
using Newtonsoft.Json;
using HTTP_API_Aplication.Interfaces;
using HTTP_API_Aplication.DTO;

namespace HTTP_API_Aplication.Data
{
    public class Repository : IRepository
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public Repository(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public List<User> GetAllUsers()
        {
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "Repository", "users.json");

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
                return users;
            }
            else
                return new List<User>();
        }

        public void AddUser(User newUser)
        {
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "Repository", "users.json");
            List<User> users = GetAllUsers();    
            users.Add(newUser);
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public void UpdateUser(User updatedUser)
        {
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "Repository", "users.json");
            List<User> users = GetAllUsers();
            User userToUpdate = users.FirstOrDefault(u => u.Id == updatedUser.Id);
            userToUpdate = updatedUser;
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public void DeleteUser(User userToDelete)
        {
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "Repository", "users.json");
            List<User> users = GetAllUsers();
            users.Remove(userToDelete);
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}

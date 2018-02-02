
namespace ChinaPay.B3B.Service.PidManagement.Domain
{
    public class User
    {
        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public string Name { get; private set; }
        public string Password { get; private set; }
    }
}

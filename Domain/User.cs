using Flunt.Notifications;
using Flunt.Validations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TaskManager.Domain;

public class User : Notifiable<Notification>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public ICollection<Tasks> UserTasks { get; set; }

    public User(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;

        Validate();
    }

    public void Validate()
    {
        var contract = new Contract<User>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsEmailOrEmpty(Email, "Email")
            .IsNotNullOrEmpty(Password, "Password");
        AddNotifications(contract);
    }
}

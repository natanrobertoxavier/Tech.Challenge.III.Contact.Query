namespace Contact.Query.Domain.Entities;
public class User(
    Guid id,
    DateTime registrationDate,
    string name,
    string email,
    string password) : BaseEntity(id, registrationDate)
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

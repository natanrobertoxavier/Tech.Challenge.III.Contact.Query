using Newtonsoft.Json;

namespace Contact.Query.Communication.Response;
public class ResponseContactJson(
    Guid contactId,
    string region,
    string firstName,
    string lastName,
    int dDD,
    string phoneNumber,
    string email,
    DateTime registrationDate)
{
    public Guid ContactId { get; set; } = contactId;
    public string Region { get; set; } = region;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public int DDD { get; set; } = dDD;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Email { get; set; } = email;
    [JsonIgnore]
    public DateTime RegistrationDate { get; set; } = registrationDate;

    public ResponseContactJson()
    : this(
          Guid.Empty,
          string.Empty,
          string.Empty,
          string.Empty,
          0,
          string.Empty,
          string.Empty,
          DateTime.MinValue)
    {
    }
}

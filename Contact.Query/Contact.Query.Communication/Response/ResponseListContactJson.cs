namespace Contact.Query.Communication.Response;
public class ResponseListContactJson(IEnumerable<ResponseContactJson> contacts)
{
    public IEnumerable<ResponseContactJson> Contacts { get; set; } = contacts;
}

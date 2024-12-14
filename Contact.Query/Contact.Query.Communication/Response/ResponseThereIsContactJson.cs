namespace Contact.Query.Communication.Response;
public class ResponseThereIsContactJson(bool thereIsContact)
{
    public bool ThereIsContact { get; set; } = thereIsContact;
}

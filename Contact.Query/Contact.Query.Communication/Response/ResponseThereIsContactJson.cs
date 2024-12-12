namespace Contact.Query.Communication.Response;
public class ResponseThereIsContactJson(bool thereIsDDDNumber)
{
    public bool ResponseThereIsContact { get; set; } = thereIsDDDNumber;
}

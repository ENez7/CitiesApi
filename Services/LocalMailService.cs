namespace CityInfo.Api.Services;

public class LocalMailService
{
    private const string MailTo = "admin@mycompany.com";
    private const string MailFrom = "noreply@mycompany.com";

    public void Send(string subject, string message)
    {
        // Send mail - output to console as we are not implementing an actual mail server
        Console.WriteLine("====================================================================");
        Console.WriteLine($"Mail from {MailFrom} to {MailTo}, with {nameof(LocalMailService)}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Message: {message}");
        Console.WriteLine("====================================================================");
    }
}
namespace CityInfo.Api.Models.Authtentication;

public class CityInfoUser
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }

    public CityInfoUser(int userId, string username, string firstName, string lastName, string city)
    {
        UserId = userId;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        City = city;
    }
}
using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.Entities.Location;

public class Country
{
    public Guid CountryId { get; set; }
    public string Title { get; set; }
    public ICollection<City> Cities { get; set; } = new List<City>();
    public ICollection<User> Users { get; set; } = new List<User>();

    public Country()
    {
    }

    //Needed for if country of the city is not found in the database
    public Country(string title)
    {
        Title = title;
    }
}
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Main;

namespace HiClass.Domain.Entities.Location;

public class City
{
    public Guid CityId { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid CountryId { get; set; }
    public Country? Country { get; set; }
    public ICollection<User> Users { get; set; } = new List<User>();
}
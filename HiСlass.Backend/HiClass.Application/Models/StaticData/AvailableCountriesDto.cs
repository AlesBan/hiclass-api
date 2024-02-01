namespace HiClass.Application.Models.StaticData;

public class AvailableCountriesDto
{
    public List<string> CountryTitles { get; set; }

    public AvailableCountriesDto(List<string> countryTitles)
    {
        CountryTitles = countryTitles;
    }
}
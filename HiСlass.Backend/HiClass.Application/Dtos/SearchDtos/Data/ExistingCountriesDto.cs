namespace HiClass.Application.Dtos.SearchDtos.Data;

public class ExistingCountriesDto
{
    public List<string> CountryTitles { get; set; }

    public ExistingCountriesDto(List<string> countryTitles)
    {
        CountryTitles = countryTitles;
    }
}
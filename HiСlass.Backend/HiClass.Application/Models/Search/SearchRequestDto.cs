namespace HiClass.Application.Models.Search;

public class SearchRequestDto
{
    public IEnumerable<string> Disciplines { get; init; } = new List<string>();
    public IEnumerable<string> Languages { get; init; } = new List<string>();
    public IEnumerable<string> Grades { get; init; } = new List<string>();
    public IEnumerable<string> Countries { get; init; } = new List<string>();
}
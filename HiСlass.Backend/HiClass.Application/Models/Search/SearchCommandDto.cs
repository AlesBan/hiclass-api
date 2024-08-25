namespace HiClass.Application.Models.Search;

public class SearchCommandDto
{
    public Guid UserId { get; init; }
    public IEnumerable<string> Disciplines { get; init; } = new List<string>();
    public IEnumerable<string> Languages { get; init; } = new List<string>();
    public IEnumerable<int> Grades { get; init; } = new List<int>();
    public IEnumerable<string> Countries { get; init; } = new List<string>();
}
namespace FitTrackApi.Core.Dto.Exercise;

public class PagedListResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}

public class ExerciseListItemResult
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Force { get; set; } = null!;
    public string Level { get; set; } = null!;
    public string Mechanic { get; set; } = null!;
    public string Equipment { get; set; } = null!;
    public string Image { get; set; } = null!;
}

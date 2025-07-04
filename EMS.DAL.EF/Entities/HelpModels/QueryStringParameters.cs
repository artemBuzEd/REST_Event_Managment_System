namespace EMS.DAL.EF.Entities.HelpModels;

public abstract class QueryStringParameters
{
    private const int MaxPageSize = 20;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? OrderBy { get; set; } = "Id";
}
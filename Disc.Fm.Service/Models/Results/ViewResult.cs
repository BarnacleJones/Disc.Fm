namespace Disc.Fm.Service.Models.Results;

public class ViewResult<T>
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }
}


namespace BusinessLayer.Models;

public class GetBlogModel
{
    public int? Id { get; set; } = 0;
    public int Page { get; set; } = 1;
    public int Take { get; set; } = 6;
    public string? Search { get; set; } = null;   
}

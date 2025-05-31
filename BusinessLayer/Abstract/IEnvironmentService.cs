namespace BusinessLayer.Abstract;

public interface IEnvironmentService
{
    bool IsProduction();
    bool IsDevelopment();
    string GetEnvironmentName();
}

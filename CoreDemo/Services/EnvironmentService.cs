using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CoreDemo.Services;

public class EnvironmentService : IEnvironmentService
{
    private readonly IWebHostEnvironment _env;

    public EnvironmentService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public bool IsProduction() => _env.IsProduction();
    public bool IsDevelopment() => _env.IsDevelopment();
    public string GetEnvironmentName() => _env.EnvironmentName;
}

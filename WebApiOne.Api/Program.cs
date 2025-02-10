using WebApiOne.Api.Operations;

namespace WebApiOne.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddScoped(typeof(IOperationHandler<>), typeof(OperationHandler<>));
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        var app = builder.Build();
        app.MapGet("log/ex1", () =>
        {
            throw new NotImplementedException();
        });
        
        app.MapGet("log/ex2", () =>
        {
            throw new ArgumentNullException();
        });
        app.MapGet("log/ex3", () =>
        {
            throw new ArgumentOutOfRangeException();
        });
        app.MapGet("log/ex4", () =>
        {
            throw new UnauthorizedAccessException();
        });
        app.MapGet("log/ex5", () =>
        {
            throw new DivideByZeroException();
        });
        app.MapGet("log/ex6", () =>
        {
            throw new DataMisalignedException();
        });
        app.MapGet("log/ex7", () =>
        {
            throw new NullReferenceException();
            
        });
        
        app.UseAuthorization();


        app.MapControllers(); 
        app.UseMiddleware<UseExceptionMiddleware>();
        app.Run();
    }
}

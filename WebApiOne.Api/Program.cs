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
        app.MapGet("log/getlogs", async (IOperationHandler<LogEntity> logOperaton) =>
        {
            string sqlQueryText = $"SELECT * FROM c ";
            var getuser = await logOperaton.GetByQueryAsync(sqlQueryText);
            
            return Results.Ok(getuser.Select(m =>
            new {
                
                Message = m.Message,
                LongMessage = m.LongMessage,
                Path = m.Path,
                Source = m.Source,
                UserId = m.UserId,
                StausCode = m.StatusCode,
                CreatedDate = m.CreateDate
            }));
        });
        
        app.UseAuthorization();


        app.MapControllers(); 
        app.UseMiddleware<UseExceptionMiddleware>();
        app.Run();
    }
}

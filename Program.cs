using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
const string _FrontEnd = "React_Origin";
const string _Public = "Public_Origin";
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(_FrontEnd, (policy) =>
    {
        policy.WithOrigins("https://localhost:3000/");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
    options.AddPolicy(_Public, (policy) =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        //policy.AllowCredentials();
    });
});
builder.Services.AddDbContext<ApplicationDBContext>((options) =>
{
    options.UseNpgsql(
       builder.Configuration.GetConnectionString("DB"), npgsqlOptions =>
       {
           npgsqlOptions.CommandTimeout(30); // seconds
           npgsqlOptions.EnableRetryOnFailure(
               maxRetryCount: 3,
               maxRetryDelay: TimeSpan.FromSeconds(5),
               errorCodesToAdd: null
           );
       }
   );
});
Console.WriteLine(builder.Configuration.GetConnectionString("DB"));
//builder.Configuration.
var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.MapControllers();
app.Run();

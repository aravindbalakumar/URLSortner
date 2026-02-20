using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
const string _FrontEnd = "React_Origin";
const string _Public = "Public_Origin";
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.ConfigureSwaggerGen()
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(_FrontEnd, (policy) =>
    {
        policy.WithOrigins("https://localhost:3000/");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        //policy.AllowCredentials();
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

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();
app.Run();

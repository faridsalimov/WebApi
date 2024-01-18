using Microsoft.EntityFrameworkCore;
using WebApiTask1.Data;
using WebApiTask1.Formatters;
using WebApiTask1.Repositories.Abstract;
using WebApiTask1.Repositories.Concrete;
using WebApiTask1.Services.Abstract;
using WebApiTask1.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("http://127.0.0.1:5500/").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddControllers(options =>
{
    options.OutputFormatters.Insert(0, new VCardOutputFormatter());           
    options.InputFormatters.Insert(0, new VcardInputFormatter());
    options.OutputFormatters.Insert(0, new CsvOutputFormatter());
    options.InputFormatters.Insert(0, new CsvInputFormatter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();    
builder.Services.AddScoped<IStudentService, StudentService>();    

var connection = builder.Configuration.GetConnectionString("myconn");
builder.Services.AddDbContext<StudentDbContext>(opt =>
{
    opt.UseSqlServer(connection);   
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x =>
x.AllowAnyMethod()
.AllowAnyHeader()
.SetIsOriginAllowed(origin => true)
.AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.Run();

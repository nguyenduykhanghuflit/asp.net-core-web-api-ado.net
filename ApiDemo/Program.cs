

using ApiDemo.Database;
using ApiDemo.Helpers;
using Quartz.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//public api cho người khác sử dụng
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));


//sử dụng AddSingleton: sẽ tạo ra một đối tượng DatabaseManager và sử dụng chung cho tất cả các request
//sử dụng AddScoped() hoặc AddTransient(): tạo ra một đối tượng mới mỗi khi có yêu cầu
//Trong constructor của các lớp mà sử dụng DatabaseManager chỉ cần thêm tham số DatabaseManager thì DI container sẽ tự động tiêm nó vào 
builder.Services.AddSingleton<DatabaseManager>();
builder.Services.AddSingleton<SqlHelper>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



using ApiDemo.Database;
using ApiDemo.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz.Util;
using System.Text;
using Grpc.Auth;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using ApiDemo.Services;

using Microsoft.EntityFrameworkCore;
using SqlTableDependency.Extensions.Providers.Sql;

using ApiDemo.SubscribeTableDependencies;
using ApiDemo.Hubs;
using ApiDemo.MiddlewareExtensions;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//public api cho người khác sử dụng
// DI

builder.Services.AddSingleton<Hub>();
builder.Services.AddSingleton<SubcribeDeal>();

builder.Services.AddSignalRCore();

var connectionString = builder.Configuration.GetConnectionString("DB_TTS");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

//sử dụng AddSingleton: sẽ tạo ra một đối tượng DatabaseManager và sử dụng chung cho tất cả các request
//sử dụng AddScoped() hoặc AddTransient(): tạo ra một đối tượng mới mỗi khi có yêu cầu
//Trong constructor của các lớp mà sử dụng DatabaseManager chỉ cần thêm tham số DatabaseManager thì DI container sẽ tự động tiêm nó vào 
builder.Services.AddSingleton<DatabaseManager>();
builder.Services.AddSingleton<SqlHelper>();
builder.Services.AddSingleton<TokenHelper>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {

        //tự cấp token
        ValidateIssuer = false,
        ValidateAudience = false,

        //ký vào token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();


builder.Services.AddSignalR();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<Hub>("/hub");

app.MapControllers();

app.UseSqlTableDependency<SubcribeDeal>(connectionString);


app.Run();

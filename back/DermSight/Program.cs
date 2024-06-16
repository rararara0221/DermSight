using System.Text;
using DermSight.Service;
using DermSight.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<JwtHelpers>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
        options.IncludeErrorDetails = true; // 預設值為 true，有時會特別關閉
    
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
            // 透過這項宣告，就可以從 "roles" 取值，並可讓 [Authorize] 判斷角色
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",

            // 一般我們都會驗證 Issuer
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

            // 通常不太需要驗證 Audience
            ValidateAudience = false,
            //ValidAudience = "JwtAuthDemo", // 不驗證就不需要填寫

            // 一般我們都會驗證 Token 的有效期間
            ValidateLifetime = true,

            // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
            ValidateIssuerSigningKey = false,

            // "1234567890123456" 應該從 IConfiguration 取得
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:UserSignKey")))
        };
    });

//設定生命週期 (有使用到Configuration都要設定)(Service)
builder.Host.ConfigureServices((hostContext,services)=>{
    // services.AddScoped<Service>();
    services.AddScoped<Forpaging>();
    services.AddScoped<MailService>();
    services.AddScoped<UserService>();
    services.AddScoped<RoleService>();
    services.AddScoped<NewsService>();
    services.AddScoped<DiseaseService>();
    services.AddScoped<ClinicService>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//授權宣告
app.UseAuthentication();
app.UseAuthorization();
//Cors設定
app.UseCors(builder =>
{
    builder.AllowAnyOrigin() // 允许任何来源
            .AllowAnyMethod() // 允许任何HTTP方法
            .AllowAnyHeader(); // 允许任何HTTP标头
});

// app.Urls.Add("http://localhost:5100");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

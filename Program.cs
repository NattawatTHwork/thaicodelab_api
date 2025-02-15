using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using thaicodelab_api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<EquipmentGroupService>();
builder.Services.AddScoped<EquipmentService>();
// builder.Services.AddScoped<EquipmentTransactionService>();
// builder.Services.AddScoped<EquipmentTransactionDetailService>();
builder.Services.AddScoped<EquipmentTypeService>();
builder.Services.AddScoped<EquipmentStatusService>();
builder.Services.AddScoped<GenderService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<RankService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<RolePermissionService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserStatusService>();
builder.Services.AddScoped<DepartmentService>();

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key must be configured.");
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero, // ถ้าไม่มีบรรทัดนี้ token จะสามารถใช้ต่อไปได้อีก 5 นาที เพื่อป้องกันความแตกต่างของเวลา
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = signingKey
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<PermissionMiddleware>();

app.MapControllers();

app.MapGet("/", () => "ThaiCodeLab API");

app.Run();

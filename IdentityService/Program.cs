using IdentityService.Configuration;
using IdentityService.Data;
using IdentityService.Services;
using IdentityService.Services.Account;
using IdentityService.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Identity DB
string? connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, UserClaimsPrincipalFactory<IdentityUser>>();

// Add services to the container.

//Start Services

builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configure JWT Autentication
IdentityJWTAuthenticationConfig.InitializeJWTBearerConfig(builder);
//Configure Identity Role Policy
IdentityPolicyConfig.InitializePolicyConfig(builder);
//Configure Swagger UI JWT
IdentitySwaggerConfig.InitializeSwaggerConfig(builder);

var app = builder.Build();

ApplicationDbContextInitializer.IdentityDbContextInitializer(app);


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

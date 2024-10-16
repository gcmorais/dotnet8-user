using System.Text;
using dotnet_user_api.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Config identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

var masterAdminConfig = builder.Configuration.GetSection("MasterAdmin");
var masterAdminEmail = masterAdminConfig["Email"];
var masterAdminPassword = masterAdminConfig["Password"];
var masterAdminFullName = masterAdminConfig["FullName"];
var masterAdminDateOfBirth = DateTime.Parse(masterAdminConfig["DateOfBirth"]);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<User>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await roleManager.RoleExistsAsync("MasterAdmin"))
        {
            await roleManager.CreateAsync(new IdentityRole("MasterAdmin"));
        }

        var masterAdminUser = await userManager.FindByEmailAsync(masterAdminEmail);
        if (masterAdminUser == null)
        {
            var masterUser = new User(masterAdminEmail, masterAdminEmail, masterAdminFullName, masterAdminDateOfBirth);

            var result = await userManager.CreateAsync(masterUser, masterAdminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(masterUser, "MasterAdmin");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating roles and master admin user: {ex.Message}");
    }
}


app.Run();

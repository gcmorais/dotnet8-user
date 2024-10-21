using dotnet_user_api.Application.Services;
using dotnet_user_api.Infrastructure.Identity;
using dotnet_user_api.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigurePersistenceApp(builder.Configuration);
builder.Services.ConfigureApplicationApp();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    // Swagger to use JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira o token JWT em formato 'Bearer {seu_token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddHttpContextAccessor();

// Configure the HTTP request pipeline.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();

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

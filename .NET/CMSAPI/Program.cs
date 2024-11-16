
using CMSAPI.Data;
using CMSAPI.Services.AuthServices;
using CMSAPI.Services.DocumentServices;
using CMSAPI.Services.FolderServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SQLitePCL;
using System.Text;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Initialize SQLite for use in the application
Batteries.Init();

// Configure controllers with JSON options to handle reference loops
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Register the application's DbContext, using SQLite as the database provider
builder.Services.AddDbContext<CMSAPIDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register custom services for document and authentication functionalities
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IFolderService, FolderService>();

// Register IHttpContextAccessor for retrieving the current user ID in services
builder.Services.AddHttpContextAccessor();

// Configure CORS (Cross-Origin Resource Sharing) to allow requests from any origin
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Configure Swagger for API documentation and enable JWT Authentication in Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure Identity and set options for user authentication
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<CMSAPIDbContext>() // Use the CMSAPIDbContext for Identity storage
    .AddDefaultTokenProviders(); // Enable default token providers for Identity

// Configure JWT authentication settings
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        options.RequireHttpsMetadata = false; // Disable HTTPS requirement for development
        var byteKey = Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value);

        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateActor = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
            ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(byteKey)
        };
    });

var app = builder.Build();

// Configure Swagger only in development environment
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware for HTTPS redirection, serving static files, and handling CORS requests
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowAll");

// Add authentication and authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Apply migrations and seed the database with initial data if necessary
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CMSAPIDbContext>();
    context.Database.Migrate(); // Apply pending migrations
    await SeedData.Initialize(services); // Seed the database with initial data if needed
}

// Run the application
app.Run();

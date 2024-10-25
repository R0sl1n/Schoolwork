using LibraryApi.Data;
using LibraryApi.Services;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=library.db"));

// Register services
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<AuthorService>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7053", "http://localhost:5232")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Initialize SQLite PCL for SQLite provider setup
Batteries.Init();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API V1"); });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(); // Enable CORS middleware
app.UseAuthorization();
app.MapControllers();

// Initialize the database with seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LibraryDbContext>();

    // Apply pending migrations and seed the database
    context.Database.Migrate();
    DbInitializer.Initialize(context);
}

app.Run();
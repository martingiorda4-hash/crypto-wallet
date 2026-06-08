using CryptoWallet.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllers();
//builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient(); // Agrega el servicio HttpClient para hacer solicitudes HTTP a otras APIs

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Define la política CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("permitirTodos", policy =>
    {
        policy.AllowAnyOrigin() // acepta peticiones en cualquier dominio
            .AllowAnyHeader()  // acepta cualquier cabecera http
            .AllowAnyMethod();//acepta cualquier método(GET,POST,PUT,DELETE,etc
    });
});
var app = builder.Build();

// Usa la política CORS
app.UseCors("permitirTodos");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
//app.MapRazorPages();

app.Run();

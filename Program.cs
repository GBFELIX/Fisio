using DbContext_PlanTributario;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Data_>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Data_") ?? throw new InvalidOperationException("Connection string 'Data_' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lancamentoes}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Lancamento",
    pattern: "{controller=Lancamentoes}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Prontuarios",
    pattern: "{controller=Prontuarios}/{action=index}/{id?}");
app.MapControllerRoute(
    name: "Pacientes",
    pattern: "{controller=Pacientes}/{action=index}/{id?}");

app.Run();

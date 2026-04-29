using DbContext_PlanTributario;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Data_>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Data_") ?? throw new InvalidOperationException("Connection string 'Data_' not found.")));

//builder.Services.AddDbContext<Data_>(options =>
//    options.UseSqlite("Data Source=Planejamento.db"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 2. A MÁGICA DO EXECUTÁVEL: Automação do Banco de Dados
// Esse bloco garante que o banco 'Planejamento.db' seja criado no PC dela
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Data_>();
        // Migrate() aplica as migrations e cria o arquivo .db automaticamente
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Se houver erro na criação do banco, ele avisa no console
        Console.WriteLine("Erro ao criar/atualizar banco SQLite: " + ex.Message);
    }
}

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

// 3. Rotas simplificadas (A rota default já cobre as outras se os nomes baterem)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Lancamentoes}/{action=Index}/{id?}");

app.Run();
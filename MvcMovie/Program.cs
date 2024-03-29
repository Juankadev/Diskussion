//Bolsa de trabajo: cree una bolsa de trabajo que permita a los empleadores publicar ofertas de trabajo y a los solicitantes de empleo postularse para puestos de trabajo.

//Realizar una solicitud de venta de entradas en l�nea.

//Ecommerce - login - listado de productos - carrito - pago(envio de mail y mensaje whatsapp?)

//Foro

using Microsoft.EntityFrameworkCore;
using Diskussion.Models;
using Diskussion.Repositories;
using Diskussion.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<IResponseRepository, ResponseRepository>();

builder.Services.AddDbContext<DiskussionDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DiskussionDbContext"));
});

builder.Services.AddSession();

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

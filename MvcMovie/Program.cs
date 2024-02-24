//Bolsa de trabajo: cree una bolsa de trabajo que permita a los empleadores publicar ofertas de trabajo y a los solicitantes de empleo postularse para puestos de trabajo.

//Realizar una solicitud de venta de entradas en línea.

//Ecommerce - login - listado de productos - carrito - pago(envio de mail y mensaje whatsapp?)

//Foro

using Microsoft.EntityFrameworkCore;
using Diskussion.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DiskussionDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DiskussionDbContext"));
});

var app = builder.Build();

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

using Microsoft.EntityFrameworkCore;
using TurboFishShop.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();  // нужно для работы с сессиями для View

builder.Services.AddSession(options =>
{
	options.Cookie.Name = ".Session";
	// options.IdleTimeout = TimeSpan.FromSeconds(10);
}); // для работы с сессиями

builder.Services.AddDbContext<ApplicationDBContext>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
	);

builder.Services.AddControllersWithViews();  // MVC

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

/* app.Use((context, next) =>
{
    context.Items["name"] = "Dany";
    return next.Invoke();
}); */

app.UseSession();  // добавление middleware для работы с сессиями

/* app.Run(x =>
{
	//return x.Response.WriteAsync("Hello " + x.Items["name"]);
	if (x.Session.Keys.Contains("name"))
	{
		return x.Response.WriteAsync("Hello " + x.Session.GetString("name"));
	}
	else
	{
		x.Session.SetString("name", "Dany");
		return x.Response.WriteAsync("no");
	}
}); */

app.Run();

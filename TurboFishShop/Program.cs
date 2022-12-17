using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TurboFishShop.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();  // нужно дл€ работы с сесси€ми дл€ View

builder.Services.AddSession(options =>
{
	options.Cookie.Name = ".Session";
	// options.IdleTimeout = TimeSpan.FromSeconds(10);
}); // дл€ работы с сесси€ми

builder.Services.AddDbContext<ApplicationDBContext>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
	);

// дл€ генерации таблиц в Ѕƒ
builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDBContext>();

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


app.UseAuthentication();  // added


app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

/* app.Use((context, next) =>
{
    context.Items["name"] = "Dany";
    return next.Invoke();
}); */

app.UseSession();  // добавление middleware дл€ работы с сесси€ми

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

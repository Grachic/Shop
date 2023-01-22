using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TurboFishShop.Data;
using TurboFishShop.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();  // ����� ��� ������ � �������� ��� View

builder.Services.AddSession(options =>
{
	options.Cookie.Name = "Session";
	// options.IdleTimeout = TimeSpan.FromSeconds(10);
}); // ��� ������ � ��������

builder.Services.AddDbContext<ApplicationDBContext>(
	options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
	);

// ��� ��������� ������ � ��
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddDefaultUI().AddDefaultTokenProviders()
	.AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>(); // EMAIL SENDER

builder.Services.AddHttpContextAccessor();

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


app.MapRazorPages();  // для определения маршрута к страницу Razor

/* app.Use((context, next) =>
{
    context.Items["name"] = "Dany";
    return next.Invoke();
}); */

app.UseSession();  // ���������� middleware ��� ������ � ��������

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

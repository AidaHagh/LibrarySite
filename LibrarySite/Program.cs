using CommonLayer.GenericClass;
using DataLayer.DbContext;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Services.Book;
using ServiceLayer.Services.BorrowBook;
using ServiceLayer.Services.ManageRequest;
using ServiceLayer.Services.News;
using ServiceLayer.Services.Payment;
using ServiceLayer.Services.UnitOfWork;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



#region DbContext

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#endregion


#region AddScoped

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IUploadFiles, UploadFiles>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowBookService, BorrowBookService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IManageRequestService, ManageRequestService>();

#endregion


#region Identity

builder.Services.AddIdentity<ApplicationUsers, ApplicationRoles>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

#endregion


#region encoder

builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Arabic }));

#endregion


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
app.UseAuthentication();



//AdminArea
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=UserManage}/{action=Index}/{id?}");

//UserArea
app.MapControllerRoute(
    name: "UserArea",
    pattern: "{area:exists}/{controller=UserHome}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

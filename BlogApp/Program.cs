using BlogApp.BusinessLayer.Abstract;
using BlogApp.BusinessLayer.Concrete;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.DataLayer.Abstract;
using BlogApp.DataLayer.Concrete.EfCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("sql_connection")));


builder.Services.AddScoped<IBlogRepository, EfBlogRepository>();
builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfUserRepository>();

builder.Services.AddScoped<IBlogService, BlogManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddScoped<IUserService, UserManager>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


SeedData.TestData(app);


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "blog_category",
    pattern: "kategori/{url}",
    defaults: new { controller = "Blog", action = "Category" }
);
app.MapControllerRoute(
    name: "blog_details",
    pattern: "blog/{url}",
    defaults: new { controller = "Blog", action = "Details" }
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Blog}/{action=Index}/{id?}");


app.MapControllers();

app.Run();

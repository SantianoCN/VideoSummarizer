using System.Text.Json;
using VideoSummarizer.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseToken("123");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "auth",
    pattern: "{controller=Authorization}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "signin",
    pattern: "{controller=Authorization}/{action=SignIn}");


app.Run();

record class Person(string name, string surname, string patronymic, int age);


static class AuthExtensions {
    public static IApplicationBuilder UseToken(this IApplicationBuilder builder, string pattern){
        return builder.UseMiddleware<AuthorizationMiddleware>(pattern); 
    }
} 
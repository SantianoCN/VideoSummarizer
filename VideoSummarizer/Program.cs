using AssemblyAI;
using VideoSummarizer;
using VideoSummarizer.Core.Contracts;
using VideoSummarizer.UseCases.Services;
using VideoSummarizer.Middlewares;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VideoSummarizer.Persistence.Implements;
using VideoSummarizer.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddMvcCore();
builder.Services.AddAuthorization();
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("AuthorizationSettings"));
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddSingleton<DataContext>();
builder.Services.AddSingleton<AccountRepository>();
builder.Services.AddSingleton<RequestRepository>();
builder.Services.AddSingleton<HashService>();
builder.Services.AddScoped<TokenValidationParameters>(o =>
{
    return new TokenValidationParameters
    {
        ValidateIssuer = true,
                ValidateAudience = true,

                ValidIssuer = builder.Configuration["AuthorizationSettings:Issuer"],
                ValidAudience = builder.Configuration["AuthorizationSettings:Audience"],


                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthorizationSettings:Key"]!))
    };
});
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<RequestService>();
builder.Services.AddSingleton<IRazorViewEngine, RazorViewEngine>();
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AssemblyAIClient>(provider => {

    var apiKey = builder.Configuration["ApiKeys:Transcription"];
    if (apiKey == "") throw new Exception("Api key is not defined");

    return new AssemblyAIClient(apiKey);
});

builder.Services.AddScoped<HttpClient>(provider => {

    // here using api static key, not iam token
    var apiKey = builder.Configuration["ApiKeys:Summarization"];
    
    var client = new HttpClient();

    client.DefaultRequestHeaders.Add("Authorization", "Api-Key " + apiKey);
    
    return client;
});

builder.Services.AddScoped<ITranscriptorService, TranscriptorService>();
builder.Services.AddScoped<ISummarizatorService, SummarizatorService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseMiddleware<FileTypeValidationMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "authorization",
    pattern: "{controller=Authorization}/{action=SignIn}/{id?}");

app.MapControllerRoute(
    name: "history",
    pattern: "{controller=History}/{action=GetUserRequests}"
);
app.MapControllerRoute(
    name: "videoProcessing",
    pattern: "{controller=VideoProcessing}/{action=Index}"
);



app.MapRazorPages();

app.Run();
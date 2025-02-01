using AssemblyAI;
using VideoSummarizer.Services;
using VideoSummarizer.Middlewares;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddMvcCore();
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "auth",
    pattern: "{controller=Authorization}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "signin",
    pattern: "{controller=Authorization}/{action=SignIn}");

app.MapControllerRoute(
    name: "videoProcessing",
    pattern: "{controller=VideoProcessing}/{action=Index}"
);

app.MapRazorPages();

app.Run();

record class Person(string name, string surname, string patronymic, int age);



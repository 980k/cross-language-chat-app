using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CrossLangChat.Data;
using CrossLangChat.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CrossLangChatContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CrossLangChatContext") ?? throw new InvalidOperationException("Connection string 'CrossLangChatContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// Add DeepLTranslationService to the container
builder.Services.AddScoped<DeepLTranslationService>(provider => 
{
    var apiKey = builder.Configuration["DEEPL_API_KEY"];
    if (apiKey == null)
    {
        throw new ArgumentNullException(nameof(apiKey), "DeepL API key is missing in configuration.");
    }
    return new DeepLTranslationService(apiKey);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

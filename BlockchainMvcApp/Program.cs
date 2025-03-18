using BlockchainMvcApp.Services;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDistributedMemoryCache(); // ✅ セッションのキャッシュストアを追加
builder.Services.AddSession(); // ✅ セッションを有効化
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<MiningService>();  // MiningServiceをDIに登録

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
app.UseSession(); // ✅ ここでセッションを有効化
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Block}/{action=Index}/{id?}");

app.Run();

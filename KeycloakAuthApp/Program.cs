using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
//using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie() // Cookie-based authentication
.AddOpenIdConnect(options =>
{
    options.Authority = "http://localhost:8080/realms/MyRealm"; // URL của Keycloak Realm
    options.ClientId = "my-dotnet-app"; // Client ID của bạn trong Keycloak
    options.ClientSecret = "dnLNrLYAhqzgM6BZXl05AZHbKTL3o39M"; // Secret nếu bạn sử dụng Client Credentials
    options.ResponseType = "code"; // Sử dụng mã phản hồi (authorization code flow)
    options.SaveTokens = true; // Lưu token vào cookie
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.RequireHttpsMetadata = false; // Tắt yêu cầu HTTPS cho môi trường phát triển

});

builder.Services.AddAuthorization(); // Thêm dịch vụ ủy quyền
// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


using api;
using Auth0.ManagementApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

var auth0Options = builder.Configuration.GetSection(Auth0Options.Key).Get<Auth0Options>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IManagementConnection, HttpClientManagementConnection>();
builder.Services.AddSingleton<Auth0ManagementApiTokenProvider>();
builder.Services.AddTransient((sp) => {
    var tokenProvider = sp.GetRequiredService<Auth0ManagementApiTokenProvider>();
    var mgmtConnection = sp.GetRequiredService<IManagementConnection>();
    var token = tokenProvider.GetTokenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    return new ManagementApiClient(token, auth0Options.Domain, mgmtConnection);
});
builder.Services.AddSingleton(auth0Options);

builder.Services.AddCors(opts =>
    opts.AddDefaultPolicy(policy =>
    policy
    .WithOrigins("https://localhost:3000")
    .AllowAnyMethod()
    .AllowAnyHeader()));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{auth0Options.Domain}/";
    options.Audience = "https://localhost:7081";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        NameClaimType = "https://localhost/email"
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

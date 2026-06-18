using System.Text;
using BanqueSang.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure();

var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new InvalidOperationException("La clé JWT est introuvable.");

var issuer = builder.Configuration["Jwt:Issuer"]
             ?? throw new InvalidOperationException("L'issuer JWT est introuvable.");

var audience = builder.Configuration["Jwt:Audience"]
               ?? throw new InvalidOperationException("L'audience JWT est introuvable.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        /// <summary>
        /// Paramètres utilisés par ASP.NET Core pour vérifier les tokens JWT reçus.
        /// Le backend vérifie la signature, l'émetteur, l'audience et la durée de validité du token.
        /// </summary>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,

            ValidateAudience = true,
            ValidAudience = audience,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// En local HTTP on laisse cette ligne commentée pour éviter l'avertissement HTTPS.
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
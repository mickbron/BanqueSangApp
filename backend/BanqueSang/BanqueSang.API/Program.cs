using System.Text;
using BanqueSang.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Ajoute les contrôleurs de l'API.
// Cela permet d'utiliser les classes Controller comme AuthController, DonneurController, etc.
builder.Services.AddControllers();

// Ajoute Swagger pour tester facilement les endpoints de l'API depuis le navigateur.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajoute les services de la couche Infrastructure.
// Cette méthode enregistre DbConnectionFactory, les repositories Dapper,
// les services JWT, BCrypt et les services métier.
builder.Services.AddInfrastructure();

// Configure CORS pour autoriser le frontend Angular à appeler l'API.
// Angular tourne généralement sur http://localhost:4200 en développement.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Récupère les informations JWT depuis appsettings.json.
// Ces valeurs servent à signer et vérifier les tokens JWT.
var jwtKey = builder.Configuration["Jwt:Key"]
             ?? throw new InvalidOperationException("La clé JWT est introuvable.");

var issuer = builder.Configuration["Jwt:Issuer"]
             ?? throw new InvalidOperationException("L'issuer JWT est introuvable.");

var audience = builder.Configuration["Jwt:Audience"]
               ?? throw new InvalidOperationException("L'audience JWT est introuvable.");

// Configure l'authentification JWT.
// Le backend vérifiera automatiquement les tokens envoyés dans le header Authorization.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Paramètres utilisés pour valider un token JWT reçu par l'API.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Vérifie que le token provient du bon émetteur.
            ValidateIssuer = true,
            ValidIssuer = issuer,

            // Vérifie que le token est destiné à la bonne application cliente.
            ValidateAudience = true,
            ValidAudience = audience,

            // Vérifie que la signature du token est correcte.
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),

            // Vérifie que le token n'est pas expiré.
            ValidateLifetime = true,

            // Supprime la tolérance par défaut de 5 minutes sur l'expiration.
            ClockSkew = TimeSpan.Zero
        };
    });

// Ajoute le système d'autorisation.
// Il sera utilisé plus tard avec [Authorize] sur les controllers.
builder.Services.AddAuthorization();

var app = builder.Build();

// Active Swagger uniquement en environnement de développement.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// En local, on peut garder cette ligne commentée pour éviter les problèmes HTTP/HTTPS.
// app.UseHttpsRedirection();

// Active la politique CORS avant l'authentification et l'autorisation.
app.UseCors("AngularPolicy");

// Active la lecture et la validation du token JWT.
app.UseAuthentication();

// Active les règles d'autorisation.
app.UseAuthorization();

// Mappe automatiquement les routes des controllers.
app.MapControllers();

// Lance l'application.
app.Run();
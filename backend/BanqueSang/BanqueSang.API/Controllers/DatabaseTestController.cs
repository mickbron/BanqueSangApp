using BanqueSang.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BanqueSang.API.Controllers;

//Ce controller est temporaire. Il sert uniquement à vérifier que Dapper fonctionne.

[ApiController]
[Route("api/database-test")]
public class DatabaseTestController : ControllerBase
{
    private readonly IDatabaseTestRepository _databaseTestRepository;

    public DatabaseTestController(IDatabaseTestRepository databaseTestRepository)
    {
        _databaseTestRepository = databaseTestRepository;
    }

    [HttpGet]
    public async Task<IActionResult> TestConnection()
    {
        var databaseName = await _databaseTestRepository.TestConnectionAsync();

        return Ok(new
        {
            message = "Connexion MySQL réussie",
            database = databaseName
        });
    }
}
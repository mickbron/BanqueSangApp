namespace BanqueSang.Core.Interfaces;

public interface IDatabaseTestRepository
{
    Task<string> TestConnectionAsync();
}
using Dapper;
using Lab2025.Models;
using Microsoft.Data.SqlClient;

namespace Lab2025.Data;

public class PalestrasRepository : Repository, IPalestrasRepository
{
    public PalestrasRepository(IConnectionStrings connectionStrings) : base(connectionStrings)
    {
    }

    public async Task<IEnumerable<PalestraModel>> ReadAsync()
    {
        const string selectAll = "SELECT * FROM [Palestra];";
        IEnumerable<PalestraModel> palestras;
        
        using (var connection = new SqlConnection(ConnectionStrings.Get("Default")))
        {
            palestras = await connection.QueryAsync<PalestraModel>(selectAll);
        }

        return palestras;
    }

    public async Task<bool> WriteAsync(PalestraModel palestra)
    {
        const string insert = """
                       INSERT INTO [Palestra] 
                              (Id, Titulo, Descricao, DataHora)
                       VALUES (@Id, @Titulo, @Descricao, @DataHora)
                       """;
        int rowsAffected;
        
        using (var connection = new SqlConnection(ConnectionStrings.Get("Default")))
        {
            rowsAffected = await connection.ExecuteAsync(insert, palestra);
        }

        return rowsAffected > 0;
    }
}
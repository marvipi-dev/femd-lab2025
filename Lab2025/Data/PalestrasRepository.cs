using Dapper;
using Lab2025.Models;
using Microsoft.Data.SqlClient;

namespace Lab2025.Data;

public class PalestrasRepository : IPalestrasRepository
{
    private readonly IConnectionStrings _connectionStrings;

    public PalestrasRepository(IConnectionStrings connectionStrings)
    {
        _connectionStrings = connectionStrings;
        CreateDbIfDoesntExist();
        CreateTablesIfDontExist();
    }

    public async Task<IEnumerable<PalestraModel>> ReadAsync()
    {
        const string selectAll = "SELECT * FROM [Palestra];";
        IEnumerable<PalestraModel> palestras;
        
        using (var connection = new SqlConnection(_connectionStrings.Get("Default")))
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
        
        using (var connection = new SqlConnection(_connectionStrings.Get("Default")))
        {
            rowsAffected = await connection.ExecuteAsync(insert, palestra);
        }

        return rowsAffected > 0;
    }
    
    
    private void CreateDbIfDoesntExist()
    {
        const string createDb =
            """
            IF NOT EXISTS (SELECT * FROM sys.databases WHERE name='Lab2025')
            BEGIN
                CREATE DATABASE [Lab2025]
            END
            """;
        using (var connection = new SqlConnection(_connectionStrings.Get("DbCreation")))
        {
            connection.Execute(createDb);
        }
    }
    
    private void CreateTablesIfDontExist()
    {
        const string createTables =
            """
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Palestra')
            BEGIN
                CREATE TABLE [Palestra]
                (
                    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                    Titulo NVARCHAR(30) NOT NULL,
                    Descricao NVARCHAR(100) NOT NULL,
                    DataHora DATETIME NOT NULL
                )
            END

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name='Participante')
            BEGIN
                CREATE TABLE [Participante]
                (
                    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
                    PalestraId UNIQUEIDENTIFIER NOT NULL FOREIGN KEY REFERENCES Palestra(Id),
                    Nome NVARCHAR(100) NOT NULL,
                    Email VARCHAR(50) NOT NULL,
                    Telefone VARCHAR(30)
                )
            END
            """;
        using (var connection = new SqlConnection(_connectionStrings.Get("Default")))
        {
            connection.Execute(createTables);
        }
    }
}
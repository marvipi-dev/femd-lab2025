using Dapper;
using Microsoft.Data.SqlClient;

namespace Lab2025.Data;

public abstract class Repository
{
    protected IConnectionStrings ConnectionStrings { get; }

    protected Repository(IConnectionStrings connectionStrings)
    {
        ConnectionStrings = connectionStrings;
        CreateDbIfDoesntExist();
        CreateTablesIfDontExist();
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
        using (var connection = new SqlConnection(ConnectionStrings.Get("DbCreation")))
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
        using (var connection = new SqlConnection(ConnectionStrings.Get("Default")))
        {
            connection.Execute(createTables);
        }
    }
}
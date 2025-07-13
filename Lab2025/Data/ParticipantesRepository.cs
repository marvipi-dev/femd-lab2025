using Dapper;
using Lab2025.Data;
using Lab2025.Models;
using Microsoft.Data.SqlClient;

public class ParticipantesRepository : Repository, IParticipantesRepository
{
    public ParticipantesRepository(IConnectionStrings connectionStrings) : base(connectionStrings)
    {
    }
    
    public async Task<IEnumerable<ParticipanteModel>> ReadAsync()
    {
        IEnumerable<ParticipanteModel> participantes;
        const string selectAll = "SELECT * FROM [Participante]";
        
        using (var connection = new SqlConnection(ConnectionStrings.Get("Default")))
        {
            participantes = await connection.QueryAsync<ParticipanteModel>(selectAll);
        }

        return participantes;
    }

    public async Task<bool> PalestraExistsAsync(Guid id)
    {
        int palestraCount;
        const string countCommand = "SELECT COUNT(*) FROM [Palestra] WHERE Id = @Id";
        
        using (var connection = new SqlConnection(ConnectionStrings.Get("Default")))
        {
            palestraCount = await connection.ExecuteScalarAsync<int>(countCommand, new { Id = id });
        }

        return palestraCount == 1;
    }

    public async Task<bool> WriteAsync(ParticipanteModel participante)
    {
        int rowsAffected;
        const string insert = """
                              INSERT INTO [Participante]
                              (Id, PalestraId, Nome, Email, Telefone)
                              VALUES (@Id, @PalestraId, @Nome, @Email, @Telefone) 
                              """;
        
        using (var connection = new SqlConnection(ConnectionStrings.Get("Default")))
        {
            rowsAffected = await connection.ExecuteAsync(insert, participante);
        }

        return rowsAffected == 1;
    }

    public async Task<bool> ParticipanteExistsAsync(Guid id)
    {
        int participanteCount;
        const string countCommand = "SELECT COUNT(*) FROM [Participante] WHERE Id = @Id";
        
        using (var connection = new SqlConnection(ConnectionStrings.Get("Default")))
        {
            participanteCount = await connection.ExecuteScalarAsync<int>(countCommand, new { Id = id });
        }

        return participanteCount == 1;
    }

    public async Task<bool> UpdateAsync(ParticipanteModel participante)
    {
        int rowsAffected;
        const string update = """
                              UPDATE [Participante]
                              SET PalestraId = @PalestraId, Nome = @Nome, Email = @Email, Telefone = @Telefone
                              WHERE Id = @Id
                              """;
        
        using (var connection = new SqlConnection(ConnectionStrings.Get("Default")))
        {
            rowsAffected = await connection.ExecuteAsync(update, participante);
        }

        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        int rowsAffected;
        const string delete = "DELETE FROM [Participante] WHERE Id = @Id";
        
        using (var connection = new SqlConnection(ConnectionStrings.Get("Default")))
        {
            rowsAffected = await connection.ExecuteAsync(delete, new { Id = id });
        }

        return rowsAffected > 0;
    }
}
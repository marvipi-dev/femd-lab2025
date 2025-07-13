using Lab2025.Models;

public interface IParticipantesRepository
{
    public Task<IEnumerable<ParticipanteModel>> ReadAsync();
    public Task<bool> PalestraExistsAsync(Guid id);
    public Task<bool> WriteAsync(ParticipanteModel participante);
    public Task<bool> ParticipanteExistsAsync(Guid id);
    public Task<bool> UpdateAsync(ParticipanteModel participante);
    public Task<bool> DeleteAsync(Guid id);
}
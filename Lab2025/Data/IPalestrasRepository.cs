using Lab2025.Models;

namespace Lab2025.Data;

public interface IPalestrasRepository
{
    public Task<IEnumerable<PalestraModel>> ReadAsync();
    public Task<bool> WriteAsync(PalestraModel palestra);
}
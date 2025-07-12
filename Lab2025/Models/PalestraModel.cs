namespace Lab2025.Models;

public class PalestraModel
{
    public required Guid Id { get; set; }
    public required string Titulo { get; set; }
    public required string Descricao { get; set; }
    public required DateTime DataHora { get; set; }
}
namespace Lab2025.Models;

public class ParticipanteModel
{
    public required Guid Id { get; set; }
    public required Guid PalestraId { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Telefone { get; set; }
}
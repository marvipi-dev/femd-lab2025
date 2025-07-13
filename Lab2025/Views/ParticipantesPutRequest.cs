namespace Lab2025.Views;

public record ParticipantesPutRequest
{
    public required Guid PalestraId { get; init; }
    public required string Nome { get; init; }
    public required string Email { get; init; }
    public required string Telefone { get; init; }
}
namespace Lab2025.Views;

public record PalestrasPostRequest()
{
    public required string Titulo { get; init; }
    public required string Descricao { get; init; }
    public required DateTime DataHora { get; init; }
}
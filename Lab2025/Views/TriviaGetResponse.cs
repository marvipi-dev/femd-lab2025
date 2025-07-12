namespace Lab2025.Views;

public record TriviaGetResponse
{
    public required IEnumerable<TriviaQuestion> Results { get; init; }
}
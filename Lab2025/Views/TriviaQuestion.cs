using System.Text.Json.Serialization;

namespace Lab2025.Views;

public record TriviaQuestion
{
    public required string Question { get; init; }
    
    [JsonPropertyName("correct_answer")]
    public required string CorrectAnswer { get; init; }
}
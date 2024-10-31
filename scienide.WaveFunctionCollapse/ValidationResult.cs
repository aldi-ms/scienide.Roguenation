namespace scienide.WaveFunctionCollapse;

public class ValidationResult(bool isValid, string message)
{
    public static readonly ValidationResult Success = new(true, "Validation passed.");

    public bool IsValid { get; set; } = isValid;
    public string Message { get; set; } = message;
}

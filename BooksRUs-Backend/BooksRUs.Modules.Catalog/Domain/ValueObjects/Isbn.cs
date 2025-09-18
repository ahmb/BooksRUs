namespace BooksRUs.Modules.Catalog.Domain.ValueObjects;

public readonly record struct Isbn
{
    public string Value { get; }
    public Isbn(string value)
    {
        if (!TryValidate(value, out var normalized)) throw new ArgumentException("Invalid ISBN", nameof(value));
        Value = normalized;
    }
    public static bool TryParse(string? raw, out Isbn isbn)
    {
        isbn = default;
        if (!TryValidate(raw, out var normalized)) return false;
        isbn = new Isbn(normalized);
        return true;
    }
    private static bool TryValidate(string? raw, out string normalized)
    {
        normalized = (raw ?? string.Empty).Trim().Replace("-", "");
        return normalized.Length is >= 10 and <= 13;
    }
    public override string ToString() => Value;
}

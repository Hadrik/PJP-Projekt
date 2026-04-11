namespace Compiler;

public class MyTypeConverter
{
    private static readonly List<TypeRecord> TypeRecords =
    [
        new() { Type = MyType.Int, Short = "I", Long = "int", Default = "0"},
        new() { Type = MyType.Float, Short = "F", Long = "float", Default = "0.0"},
        new() { Type = MyType.Bool, Short = "B", Long = "bool", Default = "false" },
        new() { Type = MyType.String, Short = "S", Long = "string", Default = "\"\"" },
        new() { Type = MyType.Error, Short = "E", Long = "error", Default = null! }
    ];

    public static TypeRecord ByType(MyType type) => TypeRecords.First(r => r.Type == type);
    public static TypeRecord ByShort(string shortName) => TypeRecords.First(r => r.Short == shortName);
    public static TypeRecord ByLong(string longName) => TypeRecords.First(r => r.Long == longName);
}

public record TypeRecord
{
    public required MyType Type { get; init; }
    public required string Short { get; init; }
    public required string Long { get; init; }
    public required string Default { get; init; }
}
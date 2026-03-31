namespace Runtime.Data;

public class Variable
{
    public required Type Type { get; init; }
    public required object Value { get; set; }
}
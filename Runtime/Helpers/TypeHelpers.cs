using System.Text.RegularExpressions;
using Type = Runtime.Data.Type;

namespace Runtime.Helpers;

public static partial class TypeHelpers
{
    public static Type? ToType(char c)
    {
        return c switch
        {
            'I' => Type.Integer,
            'F' => Type.Float,
            'B' => Type.Boolean,
            'S' => Type.String,
            _ => null
        };
    }
    
    public static bool VerifyType(string value, Type type)
    {
        return type switch
        {
            Type.Integer => IntegerRegex().IsMatch(value),
            Type.Float => FloatRegex().IsMatch(value),
            Type.Boolean => BooleanRegex().IsMatch(value),
            Type.String => StringRegex().IsMatch(value),
            _ => throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported type: {type}")
        };
    }

    [GeneratedRegex(@"^\d+$")]
    private static partial Regex IntegerRegex();
    
    [GeneratedRegex(@"^\d+\.\d+$")]
    private static partial Regex FloatRegex();
    
    [GeneratedRegex(@"^(true|false)$")]
    private static partial Regex BooleanRegex();
    
    [GeneratedRegex("^\".*\"$")]
    private static partial Regex StringRegex();
}
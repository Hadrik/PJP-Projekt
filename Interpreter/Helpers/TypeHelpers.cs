using System.Text.RegularExpressions;
using Data_Type = Interpreter.Data.Type;

namespace Interpreter.Helpers;

public static partial class TypeHelpers
{
    public static Data_Type? ToType(char c)
    {
        return c switch
        {
            'I' => Data_Type.Integer,
            'F' => Data_Type.Float,
            'B' => Data_Type.Boolean,
            'S' => Data_Type.String,
            _ => null
        };
    }
    
    public static bool VerifyType(string value, Data_Type type)
    {
        return type switch
        {
            Data_Type.Integer => IntegerRegex().IsMatch(value),
            Data_Type.Float => FloatRegex().IsMatch(value),
            Data_Type.Boolean => BooleanRegex().IsMatch(value),
            Data_Type.String => StringRegex().IsMatch(value),
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
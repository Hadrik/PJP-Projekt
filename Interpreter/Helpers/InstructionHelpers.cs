using Interpreter.Data;
using Data_Type = Interpreter.Data.Type;

namespace Interpreter.Helpers;

public static class InstructionHelpers
{
    public static void AssertType(Variable variable, Data_Type expectedType)
    {
        if (!expectedType.HasFlag(variable.Type))
            throw new InvalidOperationException($"Instruction type error. Expected {expectedType}, got {variable.Type}");
    }

    public static void AssertSameType(Variable[] variables, Data_Type? expectedType = null)
    {
        if (variables.Length == 0)
            throw new InvalidOperationException("Instruction type error. No variables provided.");

        var firstType = variables[0].Type;
        if (expectedType != null)
        {
            AssertType(variables[0], expectedType.Value);
        }

        if (variables.Any(variable => variable.Type != firstType))
        {
            throw new InvalidOperationException("Instruction type error. Variables have different types.");
        }
    }

    public static Data_Type ExpectTypeParameter(string[] parameters, bool allowMore = false)
    {
        if (!allowMore && parameters.Length != 1)
        {
            throw new InvalidOperationException($"Instruction too many parameters parameter error [{string.Join(' ', parameters)}]. Expected 1");
        }
        
        var expectedType = TypeHelpers.ToType(parameters[0][0]);
        if (expectedType == null)
        {
            throw new InvalidOperationException($"Instruction parameter type error [{parameters[0]}]. Expected {expectedType}");
        }

        return expectedType.Value;
    }
    
    public static void AssertNoParameters(string[] parameters)
    {
        if (parameters.Length != 0)
        {
            throw new InvalidOperationException($"Instruction too many parameters error [{string.Join(' ', parameters)}]. Expected 0");
        }
    }

    public static void AssertOneParameter(string[] parameters, Data_Type? expectedType = null)
    {
        if (parameters.Length != 1)
        {
            throw new InvalidOperationException($"Instruction too many parameters error [{string.Join(' ', parameters)}]. Expected 1");
        }
        
        if (expectedType != null)
        {
            if (!TypeHelpers.VerifyType(parameters[0], expectedType.Value))
            {
                throw new InvalidOperationException($"Instruction parameter type error [{parameters[0]}]. Expected {expectedType}");
            }
        }
    }
}
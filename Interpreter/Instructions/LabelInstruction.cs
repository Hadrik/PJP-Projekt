using Interpreter.Data;
using Interpreter.Helpers;
using Interpreter.Reader;

namespace Interpreter.Instructions;

public class LabelInstruction(LabelStore labelStore, IReader reader) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertOneParameter(parameters, Data.Type.Integer);
        
        var labelId = int.Parse(parameters[0]);
        labelStore.AddLabel(labelId, reader.CurrentLineNumber);
    }
}
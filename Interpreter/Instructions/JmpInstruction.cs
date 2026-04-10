using Interpreter.Data;
using Interpreter.Helpers;
using Interpreter.Reader;

namespace Interpreter.Instructions;

public class JmpInstruction(LabelStore labelStore, IReader reader) : IInstruction
{
    public void Execute(string[] parameters)
    {
        InstructionHelpers.AssertOneParameter(parameters, Data.Type.Integer);
        
        var labelId = int.Parse(parameters[0]);
        var labelLine = labelStore.GetLabelLine(labelId);
        
        reader.JumpToLine(labelLine);
    }
}
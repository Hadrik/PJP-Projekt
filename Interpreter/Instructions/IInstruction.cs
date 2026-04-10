namespace Interpreter.Instructions;

public interface IInstruction
{
    void Execute(string[] parameters);
}
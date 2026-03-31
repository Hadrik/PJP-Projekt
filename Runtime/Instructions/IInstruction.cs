namespace Runtime.Instructions;

public interface IInstruction
{
    void Execute(string[] parameters);
}
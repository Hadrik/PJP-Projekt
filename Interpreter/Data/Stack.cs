namespace Interpreter.Data;

public class Stack
{
    private readonly Stack<Variable> _stack = new();
    
    public void Push(Variable value)
    {
        _stack.Push(value);
    }
    
    public Variable Pop()
    {
        if (_stack.Count == 0)
        {
            throw new Exception("Stack underflow: No variables to pop.");
        }
        return _stack.Pop();
    }
}
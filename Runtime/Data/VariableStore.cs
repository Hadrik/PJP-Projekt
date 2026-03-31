namespace Runtime.Data;

public class VariableStore
{
    private readonly Dictionary<string, Variable> _variables = new();

    public Variable this[string name]
    {
        get
        {
            _variables.TryGetValue(name, out var value);
            return value ?? throw new Exception($"Variable '{name}' not found.");
        }
        set
        {
            if (_variables.TryGetValue(name, out var variable))
            {
                if (variable.Type != value.Type)
                {
                    throw new Exception($"Value '{value.Value}' does not match the type '{variable.Type}' for existing variable '{name}'.");
                }

                variable.Value = value.Value;
            }
            else
            {
                _variables[name] = value;
            }
        }
    }
}
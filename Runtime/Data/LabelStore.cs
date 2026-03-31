namespace Runtime.Data;

public class LabelStore
{
    private readonly Dictionary<int, int> _labes = new();
    
    public void AddLabel(int label, int line)
    {
        if (_labes.TryGetValue(label, out var oldLine))
        {
            if (oldLine == line) return;
            throw new Exception($"Label {label} already exists at line {oldLine}, cannot add at line {line}.");
        }
        _labes[label] = line;
    }
    
    public int GetLabelLine(int label)
    {
        if (!_labes.TryGetValue(label, out var line))
        {
            throw new Exception($"Label {label} not found.");
        }
        return line;
    }
}
namespace Compiler;

[Flags]
public enum MyType : int
{
    Error  = 0,
    Int    = 0b0001,
    Float  = 0b0010,
    Bool   = 0b0100,
    String = 0b1000
}
namespace Service.Exceptions
{
    public class SystemIdFromDBNotFound(string name, int value) : Exception($"Invalid type {name} value: {value}")
    {
    }
}
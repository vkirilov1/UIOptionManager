namespace Service.Exceptions
{
    public class SystemIdFromDBNotFound(String name, int value) : Exception($"Invalid type {name} value: {value}")
    {
    }
}
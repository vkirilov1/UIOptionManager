using Service.Exceptions;
using System.Reflection;

namespace Service.Others.Identifiers.Model
{
    public abstract class SystemId<T>(int value, string name) where T : SystemId<T>
    {
        public int Value { get; } = value;
        public string Name { get; } = name;

        public static T FromDatabaseValue(int value) =>
            GetAllValues().FirstOrDefault(x => x.Value == value)
            ?? throw new SystemIdFromDBNotFound($"{typeof(T).Name}", value);

        public static IEnumerable<T> GetAllValues()
        {
            return typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.FieldType == typeof(T))
                .Select(f => (T)f.GetValue(null)!);
        }
    }
}

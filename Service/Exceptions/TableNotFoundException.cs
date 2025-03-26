namespace Service.Exceptions
{
    public class TableNotFoundException(String listType) : Exception($"Error when getting table from db: No DB entry found for list: {listType}")
    {
    }
}

namespace Service.Exceptions
{
    internal class EmptyUserOptionException : Exception
    {
        public EmptyUserOptionException()
            : base("Option cannot be empty or null!")
        {
        }
    }
}
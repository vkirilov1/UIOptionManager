namespace Service.Model.Users
{
    public class User
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}

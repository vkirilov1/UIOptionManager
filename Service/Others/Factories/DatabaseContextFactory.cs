using DataLayer.Database;

namespace Service.Others.Factories
{
    public static class DatabaseContextFactory
    {
        public static DatabaseContext Create()
        {
            var context = new DatabaseContext();
            context.Database.EnsureCreated();
            return context;
        }
    }
}

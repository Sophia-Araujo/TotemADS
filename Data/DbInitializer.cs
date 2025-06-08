namespace TotemPWA.Data
{
    public static class DbInitializer
    {
        public static Task InitializeAsync(ApplicationDbContext context)
        {
            // Nenhuma inicialização necessária
            return Task.CompletedTask;
        }
    }
}

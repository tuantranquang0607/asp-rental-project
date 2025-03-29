namespace RentalsProject.Models
{
    public static class PageConfigExtension
    {
        public static int getDefaultPageSize(this IConfiguration configuration)
        {
            var configDefault = configuration["DefaultPageSize"];

            // tryparse returns true if it can cobert to int
            // and the value is retrieved from the output parameter value;
            if (!int.TryParse(configDefault, out int value)) { return int.MaxValue; }

            return value;
        }
    }
}

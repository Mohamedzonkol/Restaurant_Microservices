namespace Restaurant.Web
{
    public static class SD
    {
        public static string ProductAPIBass { get; set; }
        public static string ShoppingCartAPIBass { get; set; }
        public static string DiscountAPIBass { get; set; }
        public enum ApiType
        {
            GET,POST,PUT, DELETE
        }
    }
}

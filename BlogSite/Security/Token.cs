namespace BlogSite.Security
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; } // tokenin süresi dolduğunda yeniden token almak için kullanılır tekrar kullanıcı girişine yönlendirmez
        public DateTime Expiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }


    }
}

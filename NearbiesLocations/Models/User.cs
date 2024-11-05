namespace NearbiesLocations.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } // Korisničko ime
        public string ApiKey { get; set; } // API ključ za autentifikaciju
        public ICollection<FavoriteLocation>? FavouriteLocations { get; set; } // Lista omiljenih lokacija
        public ICollection<SearchRequest>? SearchRequests { get; set; } // Lista izvršenih pretraga
    }
}

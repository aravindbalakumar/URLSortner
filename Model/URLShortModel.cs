namespace URLSortner.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using URLSortner.Utils;
    public class URLShort
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }

        [Column("OriginalURL")]
        public string OriginalURL { get; set; }

        [Column("ShortID")]
        public string ShortID { get; set; }
        [Column("Password")]
        public string? Password { get; set; } = null!;
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        public URLShort(string OriginalURL)
        {
            this.OriginalURL = OriginalURL;
            this.ShortID = OriginalURL.ShortenURL();

        }
    }


}
public class CreateRequest
{
    public string URL { get; set; }
    public bool isSecure { get; set; } = false;
    public string? password;
}
public class ShortResponse
{
    public string URL { get; set; }
    public ShortResponse(string URL)=>this.URL=URL;
}
public class RedirectFail
{
    public string shortID { get; set; }
    public string message { get; set; }
    public RedirectFail(string shortID, string messsage="Password Required")
    {
        this.shortID = shortID;
        this.message = message;
    }
}
public class RedirectSuccess
{
    public string originalURL { get; set; }
    public RedirectSuccess(string originalURL) { this.originalURL = originalURL; }
}
public class Redirect_Password
{
    public string shortID { get; set; }
    public string password { get; set; }
}
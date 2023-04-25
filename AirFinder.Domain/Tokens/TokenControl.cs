namespace AirFinder.Domain.Tokens
{
    public class TokenControl : BaseModel
    {
        public TokenControl() { }
        public TokenControl(Guid idUser, string token, bool valid, DateTime sentDate, DateTime? expirationDate)
        {
            IdUser = idUser;
            Token = token;
            Valid = valid;
            SentDate = sentDate;
            ExpirationDate = expirationDate;
        }

        public Guid IdUser { get; set; }
        public string Token { get; set; }
        public bool Valid { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }
}

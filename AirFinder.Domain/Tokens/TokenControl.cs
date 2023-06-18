using AirFinder.Domain.Common;

namespace AirFinder.Domain.Tokens
{
    public class TokenControl : BaseModel
    {
        public TokenControl() { }
        public TokenControl(Guid idUser, string token, bool valid, long sentDate, long? expirationDate)
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
        public long SentDate { get; set; }
        public long? ExpirationDate { get; set; }

    }
}

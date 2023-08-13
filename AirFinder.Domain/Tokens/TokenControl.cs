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

        public Guid IdUser { get; set; } = Guid.Empty;
        public string Token { get; set; } = string.Empty;
        public bool Valid { get; set; } = false;
        public long SentDate { get; set; } = 0;
        public long? ExpirationDate { get; set; }

    }
}

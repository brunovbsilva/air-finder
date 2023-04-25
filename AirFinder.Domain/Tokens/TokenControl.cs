using Abp.Domain.Entities;

namespace AirFinder.Domain.Tokens
{
    public class TokenControl : Entity
    {
        public TokenControl() { }
        public TokenControl(int idUser, string token, bool valid, DateTime sentDate, DateTime? expirationDate)
        {
            IdUser = idUser;
            Token = token;
            Valid = valid;
            SentDate = sentDate;
            ExpirationDate = expirationDate;
        }

        public int IdUser { get; set; }
        public string Token { get; set; }
        public bool Valid { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }
}

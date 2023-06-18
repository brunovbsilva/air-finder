using AirFinder.Domain.GameLogs.Enums;

namespace AirFinder.Domain.Games.Models.Requests
{
    public class ListGamesRequest
    {
        public int PageIndex { get; set; } = 0;
        public int ItemsPerPage { get; set; } = 15;
        public string? JoinStatusList { get; set; } = null;
        public string? GameStatusList { get; set; } = null;
        public long? FromDate { get; set; } = null;
        public long? UpToDate { get; set; } = null;

    }
}

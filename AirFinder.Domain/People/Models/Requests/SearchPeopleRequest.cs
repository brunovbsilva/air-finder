namespace AirFinder.Domain.People.Models.Requests
{
    public class SearchPeopleRequest
    {
        public SearchPeopleRequest() { }
        public SearchPeopleRequest (
            string search,
            int pageIndex = 0,
            int itemsPerPage = 15
        )
        {
            Search = search;
            PageIndex = pageIndex;
            ItemsPerPage = itemsPerPage;
        }

        public string Search { get; set; } = string.Empty;
        public int PageIndex { get; set; } = 0;
        public int ItemsPerPage { get; set; } = 15;
    }
}

namespace Ads.WebApi.Models.Admin
{
    using Ads.Models; 

    public class AdminGetAdsBindingModel
    {
        public AdvertisementStatus? Status { get; set; }

        public int? CategoryId { get; set; }

        public int? TownId { get; set; }

        public int? StartPage { get; set; }

        public int? PageSize { get; set; }
    }
}
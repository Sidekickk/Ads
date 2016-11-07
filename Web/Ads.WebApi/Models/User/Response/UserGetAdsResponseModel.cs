namespace Ads.WebApi.Models.User.Response
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserGetAdsResponseModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public string ImageDataURL { get; set; }

        public DateTime Date { get; set; }

        public string CategoryName { get; set; }
       
        public string TownName { get; set; }
    }
}
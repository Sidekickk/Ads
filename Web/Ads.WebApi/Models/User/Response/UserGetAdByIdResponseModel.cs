namespace Ads.WebApi.Models.User.Response
{
    using Ads.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserGetAdByIdResponseModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public string ImageDataURL { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        public int? TownId { get; set; }

        public Town Town { get; set; }

    }
}
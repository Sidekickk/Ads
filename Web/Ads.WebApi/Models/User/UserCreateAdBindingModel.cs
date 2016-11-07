using System.ComponentModel.DataAnnotations;

namespace Ads.WebApi.Models.User
{
    public class UserCreateAdBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public string ImageDataURL { get; set; }

        public int? CategoryId { get; set; }

        public int TownId { get; set; }
    }
}
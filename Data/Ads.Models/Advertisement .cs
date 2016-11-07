namespace Ads.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Advertisement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get;  set; }

        [Required]
        public string Text { get; set; }

        public string ImageDataURL { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int? TownId { get; set; }

        public virtual Town Town { get; set; }

        [Required]
        public int StatusId { get; set; }

        public virtual AdvertisementStatus Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ads.WebApi.Models.User
{
    public class LoginUserBindingModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Username")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
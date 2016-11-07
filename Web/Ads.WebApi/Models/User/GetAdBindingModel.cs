using Ads.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ads.WebApi.Models.User
{
    public class GetAdBindingModel
    {

        public AdvertisementStatus? Status { get; set; }

        public int? StartPage { get; set; }

        public int? PageSize { get; set; }
    }
}
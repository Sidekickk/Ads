namespace Ads.WebApi.Controllers
{
    using System.Web.Http;
    using Models.Admin;
    using Data;
    using Ads.Models;
    using System.Data.Entity;
    using System.Linq;

    [Authorize]
    public class AdminController : ApiController
    {
        private readonly IRepository<Advertisement> ads;

        public AdminController(IRepository<Advertisement> ads)
        {
            this.ads = ads;
        }

        [HttpGet]
        [Route("Ads")]
        public IHttpActionResult GetAds([FromUri]AdminGetAdsBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var advertisements = this.ads.All()
                .Include(c => c.Category)
                .Include(t => t.Town);

            if (model.CategoryId.HasValue)
            {
                advertisements = advertisements.Where(ad => ad.CategoryId == model.CategoryId);
            }

            if (model.TownId.HasValue)
            {
                advertisements = advertisements.Where(ad => ad.TownId == model.TownId);
            }

            return this.Ok();
        }
    }
}

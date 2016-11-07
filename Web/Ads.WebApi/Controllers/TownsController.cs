namespace Ads.WebApi.Controllers
{
    using Ads.Data;
    using Ads.Models;
    using System.Linq;
    using System.Web.Http;
    using System.ComponentModel.DataAnnotations;

    public class TownsController : ApiController
    {
        private readonly IRepository<Town> towns;

        public TownsController(IRepository<Town> towns)
        {
            this.towns = towns;
        }

        [HttpGet]
        public IHttpActionResult GetTowns()
        {
            var allTowns = this.towns.All().ToList();

            if (allTowns == null)
            {
                return this.NotFound();
            }
            return this.Ok(allTowns);
        }

        [HttpGet]
        public IHttpActionResult GetTownById(int id)
        {
            var town = this.towns.All().FirstOrDefault(t => t.Id == id);

            if (town == null)
            {
                return this.NotFound();
            }

            return this.Ok(town);
        }
    }
}

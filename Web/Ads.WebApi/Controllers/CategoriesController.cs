namespace Ads.WebApi.Controllers
{
    using Ads.Models;
    using Data;
    using System.Web.Http;
    using System.Linq;
    using System.Collections.Generic;

    public class CategoriesController : ApiController
    {
        private readonly IRepository<Category> categories;

        public CategoriesController(IRepository<Category> categories)
        {
            this.categories = categories;
        }

        [HttpGet]
        public IHttpActionResult GetCategories()
        {
            var categories = this.categories.All().OrderBy(cat => cat.Id).ToList();
            return this.Ok(categories);
        }

        [HttpGet]
        public IHttpActionResult GetCategoryById(int id)
        {
            var category = this.categories.All().FirstOrDefault(cat => cat.Id == id);

            if (category == null)
            {
                return this.NotFound();
            }

            return this.Ok(category);
        }

    }
}

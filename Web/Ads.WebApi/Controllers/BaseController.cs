namespace Ads.WebApi.Controllers
{
    using Ads.Data;
    using Microsoft.AspNet.Identity;
    using System.Web.Http;

    public class BaseController : ApiController
    {
        //public BaseController(IAdsDbContext data)
        //{
        //    this.Data = (AdsDbContext)data;
        //}

        public AdsDbContext Data { get; set; }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

    }
}

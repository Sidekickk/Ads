namespace Ads.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Ads.WebApi.Models;
    using Ads.Models;
    using Data;
    using Models.User;
    using System.Text;
    using System.Linq;
    using System.Data.Entity;
    using System.Threading;
    using Models.User.Response;

    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : BaseController
    {
        private ApplicationUserManager userManager;

        // TODO: create Class for constants 

        private const int DefaultItemsPerPage = 6;

        private readonly IRepository<User> users;
        private readonly IRepository<Advertisement> ads;

        public UserController(IRepository<User> users, IRepository<Advertisement> ads)
        {
            this.users = users;
            this.ads = ads;
            this.userManager = new ApplicationUserManager(new UserStore<User>());
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager;
            }
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        // POST api/User/Ads
        [HttpPost]
        [Route("Ads")]
        public IHttpActionResult CreateNewAd(UserCreateAdBindingModel adModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.users.All().FirstOrDefault(u => u.Id == currentUserId);
            if (currentUser == null)
            {
                return this.BadRequest("Please login again..");
            }

            var AdvertisementToAdd = new Advertisement()
            {
                Title = adModel.Title,
                Text = adModel.Text,
                ImageDataURL = adModel.ImageDataURL,
                Date = DateTime.Now,
                Status = AdvertisementStatus.WaitingApproval,
                CategoryId = adModel.CategoryId,
                TownId = adModel.TownId,
                OwnerId = currentUserId
            };

            this.ads.Add(AdvertisementToAdd);

            this.ads.SaveChanges();

            return this.Ok();
        }

        // GET api/User/Ads?Status={Status}&StartPage={StartPage}&PageSize={PageSize}
        [HttpGet]
        [Route("Ads")]
        public IHttpActionResult GetAds([FromUri]GetAdBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUserId = User.Identity.GetUserId();
            var currentUser = this.users.All().FirstOrDefault(x => x.Id == currentUserId);
            if (currentUser == null)
            {
                return this.BadRequest("Please login again..");
            }

            var advertisements = this.ads.All()
                .Include(c => c.Category)
                .Include(t => t.Town)
                .Where(x => x.OwnerId == currentUserId);

            if (model.Status.HasValue)
            {
                advertisements = advertisements.Where(x => x.Status == model.Status.Value);
            }

            var ItemsPerPage = DefaultItemsPerPage;

            if (model.PageSize.HasValue)
            {
                ItemsPerPage = model.PageSize.Value;
            }

            var itemsCount = advertisements.Count();
            var pagesCount = (itemsCount + ItemsPerPage - 1) / ItemsPerPage;

            if (model.StartPage.HasValue)
            {
                advertisements.Skip(ItemsPerPage * (model.StartPage.Value - 1));
            }

            advertisements.Take(ItemsPerPage);

            advertisements.Select(ad => new UserGetAdsResponseModel
            {
                Id = ad.Id,
                Title = ad.Title,
                Text = ad.Text,
                Date = ad.Date,
                ImageDataURL = ad.ImageDataURL,
                TownName = ad.Town == null ? string.Empty : ad.Town.Name,
                CategoryName = ad.Category == null ? string.Empty : ad.Category.Name
            }).ToList();
           
            return this.Ok(advertisements);
        }

        // PUT api/User/Dactivate/{id}
        [HttpPut]
        [Route("Ads/Deactivate/{id:int}")]
        public IHttpActionResult DeactivateAd(int id)
        {
            var advertisement = this.ads.All().FirstOrDefault(ad => ad.Id == id);

            if (advertisement == null)
            {
                return this.BadRequest(string.Format("No such advertisement with ID: {0}.", id));
            }

            var currentUserId = User.Identity.GetUserId();

            if (currentUserId != advertisement.OwnerId)
            {
                return this.Unauthorized();
            }

            advertisement.Status = AdvertisementStatus.Inactive;

            this.ads.SaveChanges();

            return this.Ok(string.Format("Advetisement with title: {0} is deactivated", advertisement.Title));
        }

        // TODO: create service for deactivate and publish...

        // PUT api/User/PublishAgain/{id}
        [HttpPut]
        [Route("Ads/PublishAgain/{id:int}")]
        public IHttpActionResult PublishAd(int id)
        {
            var advertisement = this.ads.All().FirstOrDefault(ad => ad.Id == id);

            if (advertisement == null)
            {
                return this.BadRequest(string.Format("No such advertisement with ID: {0}.", id));
            }


            var currentUserId = User.Identity.GetUserId();

            if (currentUserId != advertisement.OwnerId)
            {
                return this.Unauthorized();
            }

            advertisement.Status = AdvertisementStatus.Published;

            this.ads.SaveChanges();

            return this.Ok("Advertisement is submitted  for approval");
        }


        // GET api/User/Ads/{id}
        [HttpGet]
        [Route("Ads/{id:int")]
        public IHttpActionResult GetAdById(int id)
        {
            var ad = this.ads.All()
                .Include(a => a.Category)
                .Include(a => a.Town)
                .FirstOrDefault(d => d.Id == id);

            if (ad == null)
            {
                return this.BadRequest("Advertisement do not exists");
            }

            var currentUserId = User.Identity.GetUserId();

            if (currentUserId != ad.OwnerId)
            {
                return this.Unauthorized();
            }

            var response = new UserGetAdByIdResponseModel()
            {
                Id = ad.Id,
                Title = ad.Title,
                Text = ad.Text,
                Date = ad.Date,
                ImageDataURL = ad.ImageDataURL,
                Category = ad.Category == null ? null : ad.Category,
                CategoryId = ad.CategoryId,
                Town = ad.Town == null ? null : ad.Town,
                TownId = ad.TownId,
            };

            return this.Ok(response);
        }

        // TODO: PUT api/User/Ads/{id}

        // DELETE api/User/Ads/{id}
        [HttpDelete]
        [Route("Ads/{id:int")]
        public IHttpActionResult DeleteAdById(int id)
        {
            var ad = this.ads.All().FirstOrDefault(x => x.Id == id);

            if (ad == null)
            {
                return this.BadRequest("Advertisement not found");
            }

            var currentUserId = User.Identity.GetUserId();

            if (currentUserId != ad.OwnerId)
            {
                return this.Unauthorized();
            }

            this.ads.Delete(ad);

            this.ads.SaveChanges();

            return this.Ok("Successful advertisement delete");
        }

        // POST api/User/Login
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<HttpResponseMessage> LoginUser(LoginUserBindingModel userModel)
        {
            var request = HttpContext.Current.Request;
            var token = request.Url.GetLeftPart(UriPartial.Path) 
                    + request.ApplicationPath + Startup.OAuthOptions.TokenEndpointPath;
            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string,string>("grant_type","password"),
                    new KeyValuePair<string, string>("username", userModel.Username),
                    new KeyValuePair<string, string>("password", userModel.Password)
                };

                var encodedRequestParams = new FormUrlEncodedContent(requestParams);
                var serviceTokenResponse = await client.PostAsync(token, encodedRequestParams);
                var responseString = await serviceTokenResponse.Content.ReadAsStringAsync();
                var responseCode = serviceTokenResponse.StatusCode;
                var responseMessage = new HttpResponseMessage(responseCode)
                {
                    Content = new StringContent(responseString,Encoding.UTF8,"application/json")
                };

                return responseMessage;
            }
        }

        // POST api/User/Logout
        [HttpPost]
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok("Logout successful");
        }

        // POST api/User/ChangePassword
        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return Ok("Successful password changing");
        }

        // POST api/User/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<HttpResponseMessage> Register(RegisterUserBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return await this.BadRequest(ModelState).ExecuteAsync(new CancellationToken());
            }

            var user = new User() {
                Name = model.Name,
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                TownId = model.TownId
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return await this.GetErrorResult(result).ExecuteAsync(new CancellationToken());
            }

            var loginResult = this.LoginUser(new LoginUserBindingModel()
            {
                Username = model.Username,
                Password = model.Password
            });
            return await loginResult;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.UserManager.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}

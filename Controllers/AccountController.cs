using HRTestSystem.APIResponse;
using HRTestSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace HRTestSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient client;

        public AccountController(HttpClient client)
        {
            this.client = client;
        }
        public IActionResult Login()
        {
            return View();
        } 
        
        public IActionResult Unauthorised()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.PostAsync("https://localhost:7006/api/authenticate/Applications-Authenticator", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var accessToken = await response.Content.ReadFromJsonAsync<authenticatedUser>();

                    if (accessToken.UserBelongsToApplication)
                    {
                        // Save the access token in the user's session
                        HttpContext.Session.SetString("AccessToken", accessToken.Token);
                        HttpContext.Session.SetString("UserName", accessToken.UserName);

                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Unauthorised");

                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid username or password.";
                    return View(model);
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Api Authenticator not available!";
                return View(model);
            }
        }
    }
}

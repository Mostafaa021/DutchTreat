using DutchTreat.Models;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signInManager;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(ILogger<AccountController> Logger
            , SignInManager<StoreUser> signInManager
            ,UserManager<StoreUser>userManager
            ,IConfiguration config)
        {
            _logger = Logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        public IActionResult Login()
        {
            // ask here if the identity of suer is authenticated
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName,
                    model.Password,
                   model.RememberMe,
                    false); // allow you to lock account if username and password don`t match Correctly
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                        Redirect(Request.Query["ReturnUrl"].First());
                    else
                        return RedirectToAction("Shop", "App");
                }
            }
            ModelState.AddModelError("", "Failed to login");

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout(LoginViewModel model)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        // Create Action to Create Token 
        [HttpPost]
        public async Task <IActionResult> CreateToken([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Get name of User  sent from model
                var user = await _userManager.FindByNameAsync(model.UserName);
                // if you found name 
                if (user != null)
                {
                    // to check login actually matched with password sent from model  
                    var result = await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);
                     // if matches 
                    if (result.Succeeded)
                    {
                        // Crete Toke 
                        // Declare  Claims
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub,user.Email), // subject with user email
                            new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), // jti => unique string represent eah toke so using GUID 
                            new Claim (JwtRegisteredClaimNames.UniqueName , user.UserName) // Unique name to be for User Name
                        };

                        // Create a Key to make signature to validate Token ()
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                         // Convert this Key to Signing Credentials with using Specifeid Algorithm
                        var credentials = new SigningCredentials(key , SecurityAlgorithms.HmacSha256);
                        // Create Token 

                        // Assign Audience who will use API and Issuer who sent API
                        // , Credentials , period of Token live
                        // Assign Data to token and Creating JWT  Structure
                        var Token = new JwtSecurityToken(
                            _config["Tokens:Issuer"]
                            ,_config["Tokens:Audience"]
                            , claims
                            , signingCredentials : credentials
                            , expires : DateTime.Now.AddMinutes(20));

                        // After Above Created Token convert it to string to can be as signature in header 
                        return Created("", new
                        {   // Convert Token to String to be sent to API Consumer 
                            token = new JwtSecurityTokenHandler().WriteToken(Token) ,
                            expiration = Token.ValidTo
                        }); ;
                    }
                }


                
            }
            return BadRequest();
        }
    }
}

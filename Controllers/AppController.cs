using DutchTreat.Models;
using DutchTreat.Repositories;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IDutchRepository _dutchRepo ;

        public AppController(IMailService mailService , IDutchRepository dutchRepo)
        {
            _mailService = mailService;
            _dutchRepo = dutchRepo;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("contact")] // hiding controller name in url 
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
            
             return View();
        }
        [HttpPost("contact")]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            { 
                _mailService.SendMessage("mostafa.abdelmoneim94@gmail.com", model.Subject, $"From:{model.Name}-{model.Email}, Message:{model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }
            
            return View();
        }
        [HttpGet("About")] // hiding controller name in url 

         
        public IActionResult About()
        {
            ViewBag.Title = " About Us";
            return View();
        }
        [Authorize]
        public  IActionResult Shop()
        {
            var result = _dutchRepo.GetAllProducts();
            return View(result);
        }

    }
}

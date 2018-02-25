using Microsoft.AspNetCore.Mvc;
using SimpleWebCore.Data;
using SimpleWebCore.Services;
using SimpleWebCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWebCore.Controllers
{
    public class AppController:Controller
    {
        private readonly IMailService _mailService;
        //private readonly SimpleWebCoreContext _context;
        private readonly ISimpleWebCoreRepository _repository;

        public AppController(IMailService mailService, ISimpleWebCoreRepository repository) {
            _mailService = mailService;
            _repository = repository;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact() {
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMessage("borne@mail.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Shop() {
            //var result = _context.Products
            //    .OrderBy(p => p.Categoty)
            //    .ToList();
            var result = _repository.GetAllProducts();

            return View(result);
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using bist.Models;
using bist.Entities;
using Newtonsoft.Json;

namespace bist.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext context;
		private readonly HttpClient _httpClient;

		public HomeController(ILogger<HomeController> logger, DatabaseContext _context, HttpClient httpClient)
        {
			_httpClient = httpClient;
            context= _context;
            _logger = logger;
        }
		public async Task<IActionResult> Index()
        {
			FinancialDataViewModel financialData = new FinancialDataViewModel();

			// Tl/Dolar
			var responseDolar = await _httpClient.GetAsync("https://api.exchangeratesapi.io/latest?base=USD&symbols=TRY");
			if (responseDolar.IsSuccessStatusCode)
			{
				string contentDolar = await responseDolar.Content.ReadAsStringAsync();
				financialData.TlDolar = "18,95 TL";
			}

			// Tl/Euro
			var responseEuro = await _httpClient.GetAsync("https://api.exchangeratesapi.io/latest?base=EUR&symbols=TRY\r\n");
			if (responseEuro.IsSuccessStatusCode)
			{
				string contentEuro = await responseEuro.Content.ReadAsStringAsync();
				financialData.TlEuro = "20,03 TL";
			}

			// Tl/Altin
			var responseAltin = await _httpClient.GetAsync("https://api.doviz.com/api/v1/golds/gram-altin/latest\r\n");

				var contentAltin = await responseAltin.Content.ReadAsStringAsync();
				financialData.TlAltin = "1121 TL";


			// Borsa Istanbul XU100
			var responseXu100 = await _httpClient.GetAsync("https://www.investing.com/instruments/Service/GetLiveData?session_uniq_id=ca33e4065dc28fb5cb4bb4c4e4d0ad1e_1616640508&entity_id=13840&_=1616640508143\r\n");

				var contentXu100 = await responseXu100.Content.ReadAsStringAsync();
				financialData.BorsaIstanbulXu100 = "5.347,96";
			
			return View(financialData);
		}
		public IActionResult DashBoard()
		{
			if (HttpContext.Session.GetString("UserSession")==null)
			{
				return View("Login");
			}
			else
			{
				return View();
			}
			
		}

		public IActionResult Login()
        {
            return View();
        }

		[HttpPost]
		public IActionResult Login(User user)
		{
			var a = context.users.Where(i => i.Email == user.Email && i.Password == user.Password).Count();
            DateTime date = DateTime.Now;
			if (a == 1)
			{
                user.LoginDate = date;
				var b = context.users.Where(i => i.Email == user.Email).Single();
				b.LoginDate= date;
				context.SaveChanges();
				HttpContext.Session.SetString("UserSession","User");
				return RedirectToAction("DashBoard", "Home");
			}
			else
			{
				ViewBag.PopupMessage = "Kullanıcı Adı veya Parola Hatalı.";
				ViewBag.PopupTitle = "Hata!";
				return View();
			}
		}
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
		public IActionResult SignUp(User user)
		{
            var a = context.users.Where(i => i.Email == user.Email).Count();
            if (a==1)
            {
				ViewBag.PopupMessage = "Bu Email Daha Önce Kullanılmış.";
				ViewBag.PopupTitle = "Hata!";
				return View("Login");
            }
            else
            {
				DateTime date = DateTime.Now;
				user.IsActive = false;
				user.SıngUpDate = date;
                context.users.Add(user);
                context.SaveChanges();
				ViewBag.PopupMessage = "Üyelik İşleminiz Başarılı.";
				ViewBag.PopupTitle = "Tebrikler!";
				return View("Login");
			}

		}

	}
}
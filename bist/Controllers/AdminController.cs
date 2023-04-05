using bist.Entities;
using bist.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Python.Runtime;

namespace bist.Controllers
{
    public class AdminController : Controller
    {
        private readonly DatabaseContext context;
        private readonly IAdminRepository adminRepository;
        private readonly IUserRepository userRepository;
        private readonly IHisseRepository hisseRepository;
        private readonly IWebHostEnvironment _environment;
        private readonly IPriceRepository priceRepository;

        public AdminController(IPriceRepository _priceRepository, IWebHostEnvironment environment, DatabaseContext _context, IAdminRepository _adminRepository, IUserRepository _userRepository, IHisseRepository _hisseRepository)
        {
            hisseRepository = _hisseRepository;
            userRepository = _userRepository;
            adminRepository = _adminRepository;
            context = _context;
            priceRepository = _priceRepository;
            _environment = environment;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
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
        public IActionResult Login(Admin admin)
        {
            var a = context.admins.Where(i => i.UserName == admin.UserName && i.Password == admin.Password).Count();

            if (a == 1)
            {
                HttpContext.Session.SetString("AdminSession", "Admin");
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Error = "Kullanıcı Adı veya Parola Hatalı";
                return View();
            }

        }

        public ActionResult Custom()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return View("Login");
            }
            else
            {
                return View(userRepository.GetUsers());
            }

        }
        public ActionResult Exit()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
        public IActionResult EditCustom(int Id)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return View("Login");
            }
            else
            {
                return View(userRepository.GetById(Id));
            }

        }
        [HttpPost]
        public IActionResult EditCustom(User kullanici)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return View("Login");
            }
            else
            {
                userRepository.UpdateUser(kullanici);
                return RedirectToAction(nameof(Custom));
            }

        }
        [HttpPost]
        public IActionResult DeleteCustom(int Id)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return View("Login");
            }
            else
            {
                userRepository.DeleteUser(Id);
                return RedirectToAction(nameof(Custom));
            }

        }

        public IActionResult Hisse()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return View("Login");
            }
            else
            {
                var entity = hisseRepository.GetHisses();
                return View(entity);
            }
        }
        [HttpPost]
        public IActionResult DeleteHisse(int Id)
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return View("Login");
            }
            else
            {
                hisseRepository.DeleteHisse(Id);
                return RedirectToAction(nameof(Hisse));
            }

        }
        public IActionResult CreateHisse()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return View("Login");
            }
            else
            {
                return View();
            }

        }
        [HttpGet]
        public IActionResult AddHisse()
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return View("Login");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public IActionResult AddHisse(Hisse hisse, IFormFile veriSetiDosya)
        {
                var fileName = Path.GetFileName(veriSetiDosya.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    veriSetiDosya.CopyToAsync(stream);
                }
                hisse.VeriSetiDosyaAdi = fileName;
                context.hisses.Add(hisse);
                context.SaveChanges();
                return View("Hisse",hisseRepository.GetHisses());
        }

        public IActionResult Price()
        {
            var entities = context.hisses.ToList();
            foreach (var item in entities)
            {
                string file = "wwwroot/uploads/";
                file = file + item.VeriSetiDosyaAdi;
                // Excel dosyasını okuma ve verileri depolama
                using (var package = new ExcelPackage(new FileInfo(file)))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        Console.WriteLine("Excel dosyası okunamadı.");
                        return View("Hata");
                    }

                    List<double> hacimler = new List<double>();
                    List<double> onceki_kapanis_fiyatlari = new List<double>();
                    List<double> en_yuksek_fiyatlar = new List<double>();
                    List<double> en_dusuk_fiyatlar = new List<double>();

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var hacimCell = worksheet.Cells[row, 1].Value;
                        var oncekiKapanisCell = worksheet.Cells[row, 2].Value;
                        var enYuksekFiyatCell = worksheet.Cells[row, 3].Value;
                        var enDusukFiyatCell = worksheet.Cells[row, 4].Value;

                        if (hacimCell == null || oncekiKapanisCell == null || enYuksekFiyatCell == null || enDusukFiyatCell == null)
                        {
                            Console.WriteLine($"Satır {row} verileri okunamadı. Boş değerler mevcut.");
                            continue;
                        }

                        double hacim;
                        double oncekiKapanisFiyati;
                        double enYuksekFiyat;
                        double enDusukFiyat;

                        if (!double.TryParse(hacimCell.ToString(), out hacim))
                        {
                            Console.WriteLine($"Satır {row} hacim verisi geçersiz. {hacimCell.ToString()}");
                            continue;
                        }

                        if (!double.TryParse(oncekiKapanisCell.ToString(), out oncekiKapanisFiyati))
                        {
                            Console.WriteLine($"Satır {row} önceki kapanış fiyatı verisi geçersiz. {oncekiKapanisCell.ToString()}");
                            continue;
                        }

                        if (!double.TryParse(enYuksekFiyatCell.ToString(), out enYuksekFiyat))
                        {
                            Console.WriteLine($"Satır {row} en yüksek fiyat verisi geçersiz. {enYuksekFiyatCell.ToString()}");
                            continue;
                        }

                        if (!double.TryParse(enDusukFiyatCell.ToString(), out enDusukFiyat))
                        {
                            Console.WriteLine($"Satır {row} en düşük fiyat verisi geçersiz. {enDusukFiyatCell.ToString()}");
                            continue;
                        }

                        hacimler.Add(hacim);
                        onceki_kapanis_fiyatlari.Add(oncekiKapanisFiyati);
                        en_yuksek_fiyatlar.Add(enYuksekFiyat);
                        en_dusuk_fiyatlar.Add(enDusukFiyat);
                    }

                    //PythonEngine.PythonPath = @"C:\Users\tr\AppData\Local\Programs\Python\Python39\python39.dll";
                    //PythonEngine.PythonPath = @"C:\Users\tr\AppData\Local\Programs\Python\Python39\Lib;C:\Users\tr\AppData\Local\Programs\Python\Python39\DLLs";

                    Initialize();

                    // Python kodunu çağırma
                    using (Py.GIL())
                    {
                        dynamic pythonModule = Py.Import("bist");
                        double prediction = pythonModule.predict_price(hacimler, onceki_kapanis_fiyatlari, en_yuksek_fiyatlar, en_dusuk_fiyatlar);

                        // Tahmin sonucunu kullanma
                        DateTime dateTime = DateTime.Today.AddDays(1);

                        Price price = new Price();
                        price.HisseAdi=item.HisseAdi;
                        price.TahminiFiyat = prediction;
                        price.TahminTarihi = dateTime;
                        context.prices.Add(price);
                        context.SaveChanges();
                        hacimler.Clear();
                        onceki_kapanis_fiyatlari.Clear();
                        en_yuksek_fiyatlar.Clear();
                        en_dusuk_fiyatlar.Clear();
                    }
                    PythonEngine.Shutdown();
                }

            }
            return View("PriceList");
        }
        public static void Initialize()
        {
            string pythonDll = @"C:\Users\tr\AppData\Local\Programs\Python\Python39\python39.dll";
            Environment.SetEnvironmentVariable("PYTHONNET_PYDLL", pythonDll);
            PythonEngine.Initialize();
        }

        public IActionResult PriceList() 
        {
            if (HttpContext.Session.GetString("AdminSession") == null)
            {
                return View("Login");
            }
            else
            {
                return View(priceRepository.GetPrices());
            }
            
        }
    }
}


//// Excel dosyasını okuma ve verileri depolama
//using (var package = new ExcelPackage(new FileInfo("veriler.xlsx")))
//{
//    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
//    if (worksheet == null)
//    {
//        Console.WriteLine("Excel dosyası okunamadı.");
//        return View("Hata");
//    }

//    List<double> hacimler = new List<double>();
//    List<double> onceki_kapanis_fiyatlari = new List<double>();
//    List<double> en_yuksek_fiyatlar = new List<double>();
//    List<double> en_dusuk_fiyatlar = new List<double>();

//    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
//    {
//        var hacimCell = worksheet.Cells[row, 1].Value;
//        var oncekiKapanisCell = worksheet.Cells[row, 2].Value;
//        var enYuksekFiyatCell = worksheet.Cells[row, 3].Value;
//        var enDusukFiyatCell = worksheet.Cells[row, 4].Value;

//        if (hacimCell == null || oncekiKapanisCell == null || enYuksekFiyatCell == null || enDusukFiyatCell == null)
//        {
//            Console.WriteLine($"Satır {row} verileri okunamadı. Boş değerler mevcut.");
//            continue;
//        }

//        double hacim;
//        double oncekiKapanisFiyati;
//        double enYuksekFiyat;
//        double enDusukFiyat;

//        if (!double.TryParse(hacimCell.ToString(), out hacim))
//        {
//            Console.WriteLine($"Satır {row} hacim verisi geçersiz. {hacimCell.ToString()}");
//            continue;
//        }

//        if (!double.TryParse(oncekiKapanisCell.ToString(), out oncekiKapanisFiyati))
//        {
//            Console.WriteLine($"Satır {row} önceki kapanış fiyatı verisi geçersiz. {oncekiKapanisCell.ToString()}");
//            continue;
//        }

//        if (!double.TryParse(enYuksekFiyatCell.ToString(), out enYuksekFiyat))
//        {
//            Console.WriteLine($"Satır {row} en yüksek fiyat verisi geçersiz. {enYuksekFiyatCell.ToString()}");
//            continue;
//        }

//        if (!double.TryParse(enDusukFiyatCell.ToString(), out enDusukFiyat))
//        {
//            Console.WriteLine($"Satır {row} en düşük fiyat verisi geçersiz. {enDusukFiyatCell.ToString()}");
//            continue;
//        }

//        hacimler.Add(hacim);
//        onceki_kapanis_fiyatlari.Add(oncekiKapanisFiyati);
//        en_yuksek_fiyatlar.Add(enYuksekFiyat);
//        en_dusuk_fiyatlar.Add(enDusukFiyat);
//    }

//    // Python kodunu çağırma
//    using (Py.GIL())
//    {
//        dynamic pythonModule = Py.Import("bist");
//        double prediction = pythonModule.predict_price(hacimler.ToArray(), onceki_kapanis_fiyatlari.ToArray(), en_yuksek_fiyatlar.ToArray(), en_dusuk_fiyatlar.ToArray());

//        // Tahmin sonucunu kullanma
//        Console.WriteLine($"Tahmini Fiyat: {prediction}");
//    }
//}
using Microsoft.AspNetCore.Mvc;
using pre_gamer_mvc.Infra;
using pre_gamer_mvc.Models;

namespace pre_gamer_mvc.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        private Context c = new Context();

        [TempData]
        public string Message { get; set; }

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [Route("Login")]
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        [Route("Logar")]
        public IActionResult Logar(IFormCollection form)
        {
            string email = form["Email"].ToString();
            string senha = form["Senha"].ToString();

            Jogador jogadorBuscado = c.Jogador.FirstOrDefault(j => j.Email == email && j.Senha == senha);


            if (jogadorBuscado != null)
            {
                HttpContext.Session.SetString("UserName", jogadorBuscado.Nome);
                Message = "Logado com Sucesso";
                return LocalRedirect("~/");
            }

            // Message = "Dados Inválidos";

            return LocalRedirect("~/Login/Login");
        }


        [Route("Logout")]
        public IActionResult Logout()
        {
            // HttpContext.Session.Remove("UserName");
            HttpContext.Session.Clear();

            Message = "Bye bye";
            return LocalRedirect("~/");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
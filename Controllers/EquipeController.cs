using Microsoft.AspNetCore.Mvc;
using pre_gamer_mvc.Infra;
using pre_gamer_mvc.Models;

namespace pre_gamer_mvc.Controllers
{
    [Route("[controller]")]
    public class EquipeController : Controller
    {
        private readonly ILogger<EquipeController> _logger;

        // instancia o context para acessar o banco de dados
        private readonly Context c = new Context(); //verificar

        public EquipeController(ILogger<EquipeController> logger)
        {
            _logger = logger;
        }


        // Rotas

        [Route("Listar")] // http://localhost/Equipe/Listar
        public IActionResult Index()
        {
            // recupera a Sessão
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            
            // Variável que armazena as equipes listadas
            ViewBag.Equipe = c.Equipe.ToList();
            // Retorna a view de equipe (TELA)
            return View();
        }

        [Route("Cadastrar")] // http://localhost/Equipe/Cadastrar
        public IActionResult Cadastrar(IFormCollection form)
        {
            Equipe novaEquipe = new Equipe();

            novaEquipe.Nome = form["Nome"].ToString();

            if (form.Files.Count > 0)
            {
                var file = form.Files[0];
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Equipes");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", folder, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                novaEquipe.Imagem = file.FileName;
            }
            else
            {
                novaEquipe.Imagem = "padrao.png";
            }


            c.Equipe.Add(novaEquipe);
            c.SaveChanges();

            return LocalRedirect("~/Equipe/Listar");
        }



        [Route("Editar/{id}")]
        public IActionResult Editar(int id)
        {
            // Recupera a sessão
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            
            Equipe equipe = c.Equipe.First(e => e.IdEquipe == id);

            ViewBag.Equipe = equipe;

            return View("Edit");
        }

        [Route("Atualizar")]
        public IActionResult Atualizar(IFormCollection form)
        {
            Equipe equipe = new Equipe();

            equipe.IdEquipe = int.Parse(form["idEquipe"].ToString());
            equipe.Nome = form["Nome"].ToString();

            // Upload de Imagens
            if (form.Files.Count > 0)
            {
                // Alterar a imagem
                var file = form.Files[0];
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Equipes");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                var path = Path.Combine(folder, file.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                equipe.Imagem = file.FileName;
            }
            else
            {
                // usar a imagem padrão
                equipe.Imagem = "padrao.png";
            }

            // Busca a equipe para na base de dados
            Equipe equipeBuscada = c.Equipe.First(e => e.IdEquipe == equipe.IdEquipe);
            // Console.WriteLine();
            // Console.WriteLine($"ID: {equipeBuscada.Nome}");
            // Console.WriteLine($"NOME ANTIGO: {equipeBuscada.Nome}");
            // Console.WriteLine($"IMAGEM ANTIGA: {equipeBuscada.Imagem}");
            // Console.WriteLine();
            // Console.WriteLine("***** pronto pra salvar no banco");
            // Console.WriteLine();
            

            equipeBuscada.Nome = equipe.Nome;
            equipeBuscada.Imagem = equipe.Imagem;

            // Console.WriteLine($"ID: {equipeBuscada.IdEquipe}");
            // Console.WriteLine($"NOME ANTIGO: {equipeBuscada.Nome}");
            // Console.WriteLine($"IMAGEM ANTIGA: {equipeBuscada.Imagem}");
            // Console.WriteLine();

            //atualiza a equipe com os novos dados, na base de dados
            c.Equipe.Update(equipeBuscada);
            // Confirma as alterações
            c.SaveChanges();

            return LocalRedirect("~/Equipe/Listar");
        }


        [Route("Excluir/{id}")]
        public IActionResult Excluir(int id)
        {
            Equipe equipeBuscada = c.Equipe.First(e => e.IdEquipe == id);

            c.Equipe.Remove(equipeBuscada);

            c.SaveChanges();

            return LocalRedirect("~/Equipe/Listar");
        }
        


        // Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

    }
}
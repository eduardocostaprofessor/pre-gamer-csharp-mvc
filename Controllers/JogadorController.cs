
using Microsoft.AspNetCore.Mvc;
using pre_gamer_mvc.Infra;
using pre_gamer_mvc.Models;

namespace pre_gamer_mvc.Controllers
{
    [Route("[controller]")]
    public class JogadorController : Controller
    {
        private readonly ILogger<JogadorController> _logger;
        Context c = new Context();

        public JogadorController(ILogger<JogadorController> logger)
        {
            this._logger = logger;
        }


        // Rotas
        [Route("Listar")] // http://localhost/Jogador/Listar
        public IActionResult Index()
        {
            ViewBag.Jogador = c.Jogador.ToList();
            ViewBag.Equipe = c.Equipe.ToList();

            return View();
        }

        [Route("Cadastrar")] // http://localhost/Jogador/Cadastrar
        public IActionResult Cadastrar(IFormCollection form)
        {
            Jogador novoJogador = new Jogador();

            novoJogador.Nome = form["Nome"].ToString();
            novoJogador.Email = form["Email"].ToString();
            novoJogador.Senha = form["Senha"].ToString();
            novoJogador.IdEquipe = int.Parse(form["IdEquipe"].ToString());

            c.Jogador.Add(novoJogador);
            c.SaveChanges();

            return LocalRedirect("~/Jogador/Listar");

        }
        
        [Route("Editar/{id}")] // http://localhost/Jogador/Editar
        public IActionResult Editar(int id)
        {
            Jogador jogador = c.Jogador.First(j => j.IdJogador == id);

            ViewBag.Jogador = jogador;
            ViewBag.Equipe = c.Equipe.ToList();
            return View("Edit");
        }
        
        [Route("Atualizar")] // http://localhost/Jogador/Editar
        public IActionResult Atualizar(IFormCollection form)
        {
            Jogador novoJogador = new Jogador();
            novoJogador.IdJogador = int.Parse(form["IdJogador"].ToString());
            novoJogador.Nome = form["Nome"].ToString();
            novoJogador.Email = form["Email"].ToString();
            novoJogador.Senha = form["Senha"].ToString();
            novoJogador.IdEquipe = int.Parse(form["IdEquipe"].ToString());


            Jogador jogadorBuscado = c.Jogador.First(j => j.IdJogador == novoJogador.IdJogador);

            jogadorBuscado.Nome = novoJogador.Nome;
            jogadorBuscado.Email = novoJogador.Email;
            jogadorBuscado.Senha = novoJogador.Senha;
            jogadorBuscado.IdEquipe = novoJogador.IdEquipe;

            c.Jogador.Update(jogadorBuscado);
            c.SaveChanges();

            return LocalRedirect("~/Jogador/Listar");
        }
        
        [Route("Excluir/{id}")] // http://localhost/Jogador/Listar
        public IActionResult Excluir(int id)
        {
            Jogador jogador = c.Jogador.First(j => j.IdJogador == id);

            c.Jogador.Remove(jogador);
            c.SaveChanges();

            return LocalRedirect("~/Jogador/Listar");
        }
    }
}
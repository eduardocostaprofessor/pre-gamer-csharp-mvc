using System.ComponentModel.DataAnnotations;

namespace pre_gamer_mvc.Models
{
    public class Equipe
    {
        [Key]
        public int IdEquipe { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }

        // Referencia que a classe equipe vai ter acesso à collection Jogador
        public ICollection<Jogador> Jogador { get; set; }
    }
}
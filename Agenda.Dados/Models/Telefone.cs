
namespace Agenda.Dados.Models
{
    public class Telefone : Base
    {
        public string Numero { get; set; }
        public string Tipo { get; set; }

        public enum TiposTelefone
        {
            Casa,
            Trabalho,
            Outro
        }        
    }
}
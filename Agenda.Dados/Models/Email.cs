namespace Agenda.Dados.Models
{
    public class Email : Base
    {
        public string DesEmail { get; set; }
        public string Tipo { get; set; }

        public enum TiposEmail
        {
            Casa,
            Trabalho,
            Outro
        }
    }
}
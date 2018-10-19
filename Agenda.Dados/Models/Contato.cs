using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agenda.Dados.Models
{
    public class Contato : Base
    {
        public Contato()
        {
            Telefones = new List<Telefone>();
            Emails = new List<Email>();
        }

        public string Nome { get; set; }
        public List<Telefone> Telefones { get; set; }
        public List<Email> Emails { get; set; }
        public string Empresa {get;set;}
        public string Endereco { get; set; }

        public enum TiposPesquisa
        {
            Nome,
            Telefone,
            EmailPesquisa
        }
    }    
}
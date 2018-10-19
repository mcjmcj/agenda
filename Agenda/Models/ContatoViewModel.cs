using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agenda.Models
{
    public class ContatoViewModel
    {
        public ContatoViewModel()
        {
            Emails = new List<Email>();
            Telefones = new List<Telefone>();
        }

        public long Codigo { get; set; }
        public string Nome { get; set; }
        public string Empresa {get;set;}
        public string Endereco { get; set; }

        public List<Email> Emails { get; set; }

        public List<Telefone> Telefones { get; set; }

        
    }    
}
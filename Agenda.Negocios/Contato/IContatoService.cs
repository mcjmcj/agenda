using Agenda.Dados.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Dados.Service
{
    public interface IContatoService
    {
        string MontarWhere(IEnumerable<Contato> filtro);

        List<Contato> ListarContatos(string pesquisa, string tipo);

        Contato ConsultarContato(long codigo);

        void InserirContatos(Contato contato);

        void ExcluirContato(int codigo);
    }
}

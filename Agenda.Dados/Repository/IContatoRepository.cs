using Agenda.Dados.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agenda.Dados.Repository
{
    public interface IContatoRepository
    {
        List<Contato> ListarContatos(string pesquisa, string tipo);

        Contato ConsultarContato(long codigo);

        void InserirContatos(Contato contato);

        void ExcluirContato(long codigo);
    }
}

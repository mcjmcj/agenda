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
    public class ContatoRepository : IContatoRepository
    {
        public string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        private string MontarWhere(IEnumerable<Contato> filtro)
        {
            var whereContatoLista = "";
            var whereAux = "";

            foreach (var f in filtro)
            {
                whereAux = string.Concat(whereAux, f.Codigo.ToString(), ",");
            }
            if (whereAux.Length > 0)
            {
                whereAux = whereAux.Substring(0, whereAux.Length - 1);
                whereContatoLista = string.Format("WHERE codigo IN ({0})", whereAux);
            }
            return whereContatoLista;
        }

        public List<Contato> ListarContatos(string pesquisa, string tipo)
        {
            List<Contato> contatos = new List<Contato>();

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                var whereContatoLista = "";

                if (!string.IsNullOrEmpty(pesquisa))
                {
                    if (tipo == "Nome")
                    {
                        var filtro = sqlConnection.Query<Contato>("Select Codigo, Nome from vw_contatos WHERE nome like @pesquisa", new { pesquisa = string.Concat("%",pesquisa,"%") });
                        whereContatoLista = MontarWhere(filtro);
                    }
                    else if (tipo == "Telefone")
                    {
                        var filtro = sqlConnection.Query<Contato>($"select a.codigo from contato a inner join contatotelefone b on b.codigo = a.codigo where replace(replace(replace(replace(b.numero, ')', ''), '(', ''), '-', ''), ' ', '') like @pesquisa", new { pesquisa = string.Concat("%", pesquisa, "%") });
                        whereContatoLista = MontarWhere(filtro);
                    }
                    else if (tipo == "EmailPesquisa")
                    {
                        var filtro = sqlConnection.Query<Contato>($"select a.codigo from contato a inner join contatoemail b on b.codigo = a.codigo where b.desemail like @pesquisa", new { pesquisa = string.Concat("%", pesquisa, "%") });
                        whereContatoLista = MontarWhere(filtro);
                    }

                }

                var result = sqlConnection.Query<Contato>($"Select Codigo, Nome from vw_contatos {whereContatoLista}  ORDER BY nome");
                var resultTelefones = sqlConnection.Query<Telefone>("Select Codigo, Numero, Tipo from ContatoTelefone");
                var resultEmails = sqlConnection.Query<Email>("Select Codigo, DesEmail, Tipo from ContatoEmail");

                foreach (Contato contato in result)
                {
                    contato.Telefones = new List<Telefone>();
                    foreach (var tel in resultTelefones.Where(x => x.Codigo == contato.Codigo))
                    {
                        contato.Telefones.Add(new Telefone()
                        {
                            Codigo = tel.Codigo,
                            Numero = tel.Numero,
                            Tipo = tel.Tipo
                        });
                    }

                    contato.Emails = new List<Email>();
                    foreach (var email in resultEmails.Where(x => x.Codigo == contato.Codigo))
                    {
                        contato.Emails.Add(new Email()
                        {
                            Codigo = email.Codigo,
                            DesEmail = email.DesEmail,
                            Tipo = email.Tipo
                        });
                    }

                    contatos.Add(contato);
                }
            }

            return contatos;
        }

        public Contato ConsultarContato(long codigo)
        {
            List<Contato> contatos = new List<Contato>();
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                var result = sqlConnection.Query<Contato>($"SELECT Codigo, Nome, Empresa, Endereco FROM Contato WHERE codigo = @codigo", new { codigo });
                var resultTelefones = sqlConnection.Query<Telefone>($"SELECT Codigo, Numero, Tipo FROM ContatoTelefone WHERE codigo = @codigo", new { codigo });
                var resultEmails = sqlConnection.Query<Email>($"SELECT Codigo, DesEmail, Tipo FROM ContatoEmail WHERE codigo = @codigo", new { codigo });

                foreach (Contato contato in result)
                {
                    contato.Telefones = new List<Telefone>();
                    foreach (var tel in resultTelefones.Where(x => x.Codigo == contato.Codigo))
                    {
                        contato.Telefones.Add(new Telefone()
                        {
                            Codigo = tel.Codigo,
                            Numero = tel.Numero,
                            Tipo = tel.Tipo
                        });
                    }

                    contato.Emails = new List<Email>();
                    foreach (var email in resultEmails.Where(x => x.Codigo == contato.Codigo))
                    {
                        contato.Emails.Add(new Email()
                        {
                            Codigo = email.Codigo,
                            DesEmail = email.DesEmail,
                            Tipo = email.Tipo
                        });
                    }

                    contatos.Add(contato);
                }
            }

            return contatos.FirstOrDefault();
        }

        public void InserirContatos(Contato contato)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                if (contato.Codigo == 0)
                {
                    var result = sqlConnection.Query<GeradorId>("[dbo].[consultar_id] 'contato'");
                    contato.Codigo = result.FirstOrDefault().Codigo;
                    sqlConnection.Execute("INSERT INTO contato (codigo, nome, endereco, empresa) VALUES (@Codigo, @Nome, @Endereco, @Empresa)", contato);

                    foreach (var tel in contato.Telefones)
                    {
                        tel.Codigo = contato.Codigo;
                        sqlConnection.Execute("INSERT INTO ContatoTelefone (codigo, numero, tipo) VALUES (@Codigo, @Numero, @Tipo)", tel);
                    }

                    foreach (var mail in contato.Emails)
                    {
                        mail.Codigo = contato.Codigo;
                        sqlConnection.Execute("INSERT INTO ContatoEmail (codigo, desEmail, tipo) VALUES (@Codigo, @DesEmail, @Tipo)", mail);
                    }
                }
                else
                {
                    sqlConnection.Execute("UPDATE contato SET nome = @Nome, endereco = @Endereco, empresa = @Empresa WHERE codigo = @Codigo", contato);
                    sqlConnection.Execute("DELETE FROM contatoTelefone WHERE codigo = @Codigo", contato);
                    sqlConnection.Execute("DELETE FROM contatoEmail WHERE codigo = @Codigo", contato);
                    foreach (var tel in contato.Telefones)
                    {
                        tel.Codigo = contato.Codigo;
                        sqlConnection.Execute("INSERT INTO ContatoTelefone (codigo, numero, tipo) VALUES (@Codigo, @Numero, @Tipo)", tel);
                    }

                    foreach (var mail in contato.Emails)
                    {
                        mail.Codigo = contato.Codigo;
                        sqlConnection.Execute("INSERT INTO ContatoEmail (codigo, desEmail, tipo) VALUES (@Codigo, @DesEmail, @Tipo)", mail);
                    }
                }
            }
        }

        public void ExcluirContato(long codigo)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Execute($"DELETE FROM contatoEmail WHERE codigo = @codigo", new { codigo });
                sqlConnection.Execute($"DELETE FROM contatoTelefone WHERE codigo = @codigo", new { codigo });
                sqlConnection.Execute($"DELETE FROM contato WHERE codigo = @codigo", new { codigo });
            }
        }


    }
}

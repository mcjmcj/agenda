using System;
using Agenda.Dados.Models;
using Agenda.Dados.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System.Web.Mvc;
using SimpleInjector.Integration.Web.Mvc;

namespace Agenda.Teste
{
    [TestClass]
    public class AgendaTest
    {
        private IContatoRepository _repositorio;

        public AgendaTest()
        {
            var container = new Container();
            container.Register<IContatoRepository, ContatoRepository>(Lifestyle.Singleton);
            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            _repositorio = container.GetInstance<IContatoRepository>();
        }


        [TestMethod]
        public void InserirContato()
        {
            string mensagem = "";
            try
            {
                Contato contato = new Contato()
                {
                    Codigo = 0,
                    Nome = $"contato {new Random(100).Next()} de teste",
                    Empresa = "empresa teste",
                    Endereco = "endereco de teste"
                };

                _repositorio.InserirContatos(contato);
            }
            catch(Exception ex)
            {
                mensagem = ex.Message;
            }
            
            Assert.AreEqual(
                true,
                mensagem.Length.Equals(0), 
                "Contato gravado com sucesso");
        }

        [TestMethod]
        public void ListarContatos()
        {
            string mensagem = "";
            try
            {
                _repositorio.ListarContatos(null, null);
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }

            Assert.AreEqual(
                true,
                mensagem.Length.Equals(0),
                "Contato listados com sucesso");
        }

        [TestMethod]
        public void ExcluirContato()
        {
            string mensagem = "";
            try
            {
                var contatos = _repositorio.ListarContatos(null, null);
                if (contatos.Count > 0)
                    _repositorio.ExcluirContato(contatos[0].Codigo);
            }
            catch (Exception ex)
            {
                mensagem = ex.Message;
            }

            Assert.AreEqual(
                true,
                mensagem.Length.Equals(0),
                "Contato excluido com sucesso");
        }
    }
}

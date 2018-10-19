using Agenda.Dados.Repository;
using System.Web.Mvc;

namespace Agenda.Controllers
{
    public class HomeController : Controller
    {
        private IContatoRepository _repositorio;

        public HomeController(IContatoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public ActionResult Index(string pesquisa, string tipo)
        {
            var contatos = _repositorio.ListarContatos(pesquisa, tipo);

            return View("Index", contatos);
        }
    }
}

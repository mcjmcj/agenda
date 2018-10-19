using Agenda.Dados.Models;
using Agenda.Dados.Repository;
using System.Web.Mvc;

namespace Agenda.Controllers
{
    public class ContatoController : Controller
    {
        private IContatoRepository _repositorio;

        public ContatoController(IContatoRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public ActionResult Novo()
        {
            Contato contatoViewModel = new Contato();
            return View(contatoViewModel);
        }

        [HttpPost]
        public ActionResult Salvar(Contato vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Criar", vm);
            }           

            _repositorio.InserirContatos(vm);

            return RedirectToAction("Index","Home");
        }

        public ActionResult Editar(long codigo)
        {
            var contato = _repositorio.ConsultarContato(codigo);
            return View("Editar", contato);
        }

        public ActionResult Excluir(int codigo)
        {
            _repositorio.ExcluirContato(codigo);
            return RedirectToAction("Index", "Home");
        }



    }
}

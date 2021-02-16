using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Agenda.Models;
using Web.Agenda.Data;
using System.Net;

namespace Web.Agenda.Controllers
{
    public class HomeController : Controller
    {
        Repository repository = null;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tarefas()
        {
            return View();
        }

        public ActionResult NovaTarefa()
        {
            var res = new List<TarefaMod>()
            {
                new TarefaMod{Id=1,Nome="Sim"},
                new TarefaMod{Id=2, Nome="Não"}
            };
            ViewBag.ClienteId = new SelectList(res, "Id", "Nome");
            return View();
        }
 
       [HttpPost]
       public ActionResult NovaTarefa(TarefaMod tarefaMod)
        {
            repository = new Repository();
            try
            {
                if (ModelState.IsValid)
                {

                    if (repository.Create(tarefaMod))
                    {
                        TempData["Sucesso"] = "Tarefa Cadastrada com sucesso!!";
                    }
                        

                }
                else
                {
                    ViewBag.Falha = "O cadastro da tarefa não foi realizada";

                    return View();
                }
            }
            catch (Exception ex)
            {

                ViewBag.Falha ="[ERRO] -  Verifique:" + ex.Message;
            }
            return RedirectToAction("NovaTarefa");
        }

        public ActionResult ListarTodasTarefas()
        {
            List<TarefaMod> listaTarefa = null;

            repository = new Repository();

            try
            {
                listaTarefa = repository.ReadAll();
            }
            catch (Exception)
            {

                ViewBag.Falha = "O cadastro da tarefa não foi realizado :(";

                return RedirectToAction("Index");
            }
            return View(listaTarefa);
        }

        public ActionResult Detalhes (int? id)
        {
            TarefaMod tarefa = null;

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                repository = new Repository();

                tarefa = repository.Read(Convert.ToInt32(id));
                if (tarefa == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {

                ViewBag.Falha ="O Cadastro da tarefa não foi realizado" + ex.Message;
                return ViewBag();
            }
            return View(tarefa);
        }

        public ActionResult Editar (int id = 0)
        {
            TarefaMod tarefa = null;
            try
            {
                if (id <= 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                repository = new Repository();
                tarefa = repository.Read(id);
                if (tarefa == null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {

                ViewBag.Falha = "O cadastro da tarefa não foi realizado" + ex.Message;
                return ViewBag();
            }
            return View(tarefa);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(TarefaMod tarefas)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repository = new Repository();
                    repository.Update(tarefas);
                    return RedirectToAction("ListarTodasTarefas");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View(tarefas);
        }

        public ActionResult Excluir(int? id)
        {
            TarefaMod tarefa = null;

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                repository = new Repository();
                tarefa = repository.Read(Convert.ToInt32(id));
                if (tarefa ==null)
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Falha = "O item não foi excluido" + ex.Message;
                ViewBag();
            }
            return View(tarefa);
        }

        [HttpPost,ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult ExcluirConfirar(int id)
        {
            TarefaMod tarefa = null;

            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                repository = new Repository();

                tarefa = repository.Read(Convert.ToInt32(id));
                if (tarefa == null)
                {
                    return HttpNotFound();
                }
                repository.Delete(id);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("ListarTodasTarefas");


        }
    }
}
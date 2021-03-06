using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Tarefas.Dominio.Models;
using Tarefas.Dominio.Repositorio;
using Tarefas.Infra.Repositorio;
using TarefasSite.HttpContext;
using TarefasSite.ViewModels;

namespace TarefasSite.Controllers
{
    [Authorize]
    public class TarefasController : Controller
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly ITarefaRepositorio _tarefaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUserContext _userContext;

        public TarefasController(ICategoriaRepositorio categoriaRepositorio, ITarefaRepositorio tarefaRepositorio, IUsuarioRepositorio usuarioRepositorio, IUserContext userContext)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _tarefaRepositorio = tarefaRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _userContext = userContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                string email = _userContext.GetUserEmail();

                //buscar os registros da tabela de tarefas no repositorio
                List<Tarefa> tarefas = _tarefaRepositorio.Buscar(email);

                //Lista de Categorias
                List<Categoria> categorias = _categoriaRepositorio.Buscar();

                List<ListaTarefasViewModel> tarefasViewModels = new List<ListaTarefasViewModel>();

                foreach (var tarefa in tarefas)
                {
                    ListaTarefasViewModel tarefaViewModel = new ListaTarefasViewModel();

                    tarefaViewModel.Id = tarefa.Id;
                    tarefaViewModel.Descricao = tarefa.Descricao;
                    tarefaViewModel.Data = tarefa.Data;
                    tarefaViewModel.Notificacao = tarefa.Notificacao;

                    foreach (var categoria in categorias)
                    {
                        if (categoria.Id == tarefa.IdCategoria)
                        {
                            tarefaViewModel.Categoria = categoria.Descricao;
                        }
                    }

                    tarefasViewModels.Add(tarefaViewModel);
                }
                return View(tarefasViewModels);

            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
             return BuscarTarefa(id);
        }

        [HttpGet]
        public IActionResult Nova()
        {
            try
            {
                TarefaViewModel tarefa = new TarefaViewModel();
                tarefa.Data = DateTime.Now;
                tarefa.Hora = DateTime.Now;

                tarefa.Categorias = BuscarCategorias();
                tarefa.Convidados = BuscarConvidados();

                return View(tarefa);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        [HttpPost]
        public IActionResult Nova(TarefaViewModel tarefaViewModel)
        {
            try
            {

                if (ModelState.IsValid)
                {


                    DateTime dataHora = new DateTime(tarefaViewModel.Data.Year, tarefaViewModel.Data.Month, tarefaViewModel.Data.Day, tarefaViewModel.Hora.Hour, tarefaViewModel.Hora.Minute, 0);
                    string email = _userContext.GetUserEmail();

                    Tarefa tarefa = new Tarefa(dataHora, tarefaViewModel.Descricao, tarefaViewModel.Notificacao, tarefaViewModel.IdCategoria.Value, email);
                    tarefa.Convidados = tarefaViewModel.EmailConvidado;

                    _tarefaRepositorio.Inserir(tarefa);

                    return RedirectToAction("Inserir");
                }
                else
                {
                    ViewBag.ExisteErro = true;

                    tarefaViewModel.Categorias = BuscarCategorias();

                    return View(tarefaViewModel);
                }
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        [HttpGet]
        public IActionResult Inserir()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Excluir(int id)
        {
             return BuscarTarefa(id);
        }

        [HttpPost]
        public IActionResult Editar(TarefaViewModel tarefaViewModel)
        {
            try
            {
                DateTime dataHora = new DateTime(tarefaViewModel.Data.Year, tarefaViewModel.Data.Month, tarefaViewModel.Data.Day, tarefaViewModel.Hora.Hour, tarefaViewModel.Hora.Minute, 0);

                string email = _userContext.GetUserEmail();

                Tarefa tarefa = new Tarefa(tarefaViewModel.Id, dataHora, tarefaViewModel.Descricao, tarefaViewModel.Notificacao, tarefaViewModel.IdCategoria.Value, email);
                tarefa.Convidados = tarefaViewModel.EmailConvidado;
                _tarefaRepositorio.Atualizar(tarefa, email);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        [HttpGet]
        public IActionResult Deletar(int id)
        {
            try
            {
                string email = _userContext.GetUserEmail();
                _tarefaRepositorio.Excluir(id, email);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Detalhes(int id)
        {
            return BuscarTarefa(id);
        }

        private IActionResult BuscarTarefa(int id)
        {
            try
            {

                string email = _userContext.GetUserEmail();

                //buscar tarefa no repositorio

                Tarefa tarefa = _tarefaRepositorio.Buscar(id, email);

                TarefaViewModel tarefaViewModel = new TarefaViewModel();
                tarefaViewModel.Categorias = BuscarCategorias();
                tarefaViewModel.Convidados = BuscarConvidados();

                if (tarefa == null)
                {
                    //erro >> tarefa nao encontrada
                    ViewBag.ExisteErro = true;
                    this.ModelState.AddModelError("Tarefa_Nao_Encontrada", "Tarefa não encontrada");
                    return View(tarefaViewModel);
                }

                tarefaViewModel.Id = tarefa.Id;
                tarefaViewModel.Descricao = tarefa.Descricao;
                tarefaViewModel.Data = tarefa.Data;
                tarefaViewModel.Hora = tarefa.Data;
                tarefaViewModel.Notificacao = tarefa.Notificacao;
                tarefaViewModel.IdCategoria = tarefa.IdCategoria;
                tarefaViewModel.EmailConvidado = tarefa.Convidados;

                //Carregar lista de Categorias
                tarefaViewModel.Categorias = BuscarCategorias();

                return View(tarefaViewModel);
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        private List<SelectListItem> BuscarCategorias()
        {

            List<Categoria> categorias = _categoriaRepositorio.Buscar();

            List<SelectListItem> listItems = new List<SelectListItem>();

            for (int i = 0; i < categorias.Count; i++)
            {
                Categoria categoria = categorias[i];

                SelectListItem item = new SelectListItem(categoria.Descricao, categoria.Id.ToString());

                listItems.Add(item);
            }

            return listItems;
        }



        private MultiSelectList BuscarConvidados()
        {
            List<Usuario> convidados = _usuarioRepositorio.Buscar();

            MultiSelectList listItems = new MultiSelectList(convidados, "Email", "Email");

            return listItems;
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message
            };
            return View(viewModel);
        }
    }
}

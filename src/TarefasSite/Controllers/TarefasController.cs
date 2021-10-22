using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Tarefas.Dominio.Models;
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
        private readonly IUserContext _userContext;

        public TarefasController(ICategoriaRepositorio categoriaRepositorio, ITarefaRepositorio tarefaRepositorio, IUserContext userContext)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _tarefaRepositorio = tarefaRepositorio;
            _userContext = userContext;
        }

        [HttpGet]
        public IActionResult Index()
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
                    if(categoria.Id == tarefa.IdCategoria)
                    {
                        tarefaViewModel.Categoria = categoria.Descricao;
                    }
                }

                tarefasViewModels.Add(tarefaViewModel);            }

            return View(tarefasViewModels);
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            return BuscarTarefa(id);
        }

        [HttpGet]
        public IActionResult Nova()
        {
            TarefaViewModel tarefa = new TarefaViewModel();
            tarefa.Data = DateTime.Now;
            tarefa.Hora = DateTime.Now;

            tarefa.Categorias = BuscarCategorias();

            return View(tarefa);
        }

        [HttpPost]
        public IActionResult Nova(TarefaViewModel tarefaViewModel)
        {
            if (ModelState.IsValid)
            {
                var listaUsuarios = new List<Usuario>();

                Usuario usuario = new Usuario("Maria", "maria@gmail.com", "12345678");
                listaUsuarios.Add(usuario);
                Usuario usuario1 = new Usuario("Juliana", "juliana@gmail.com", "12345");
                listaUsuarios.Add(usuario1);
                Usuario usuario2 = new Usuario("Carlos", "carlos@gmail.com", "abcde");
                listaUsuarios.Add(usuario2);
                Usuario usuario3 = new Usuario("Paulo", "paulo@gmail.com", "123abc");
                listaUsuarios.Add(usuario3);
                Usuario usuario4 = new Usuario("José", "jose@gmail.com", "abc123");
                listaUsuarios.Add(usuario4);

                DateTime dataHora = new DateTime(tarefaViewModel.Data.Year, tarefaViewModel.Data.Month, tarefaViewModel.Data.Day, tarefaViewModel.Hora.Hour, tarefaViewModel.Hora.Minute, 0);
                string email = _userContext.GetUserEmail();

                Tarefa tarefa = new Tarefa(dataHora, tarefaViewModel.Descricao, tarefaViewModel.Notificacao, tarefaViewModel.IdCategoria.Value, email);
                tarefa.Convidados = listaUsuarios;
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
            DateTime dataHora = new DateTime(tarefaViewModel.Data.Year, tarefaViewModel.Data.Month, tarefaViewModel.Data.Day, tarefaViewModel.Hora.Hour, tarefaViewModel.Hora.Minute, 0);

            string email = _userContext.GetUserEmail();

            Tarefa tarefa = new Tarefa(tarefaViewModel.Id, dataHora, tarefaViewModel.Descricao, tarefaViewModel.Notificacao, tarefaViewModel.IdCategoria.Value, email);

            _tarefaRepositorio.Atualizar(tarefa, email);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Deletar(int id)
        {
            string email = _userContext.GetUserEmail();
            _tarefaRepositorio.Excluir(id, email);

            return RedirectToAction("Index");
        }

        public IActionResult Detalhes(int id)
        {
            return BuscarTarefa(id);
        }

        private IActionResult BuscarTarefa(int id)
        {
            string email = _userContext.GetUserEmail();

            //buscar tarefa no repositorio

            Tarefa tarefa = _tarefaRepositorio.Buscar(id, email);

            TarefaViewModel tarefaViewModel = new TarefaViewModel();
            tarefaViewModel.Categorias = BuscarCategorias();

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

            //Carregar lista de Categorias
            tarefaViewModel.Categorias = BuscarCategorias();

            return View(tarefaViewModel);
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
    }
}

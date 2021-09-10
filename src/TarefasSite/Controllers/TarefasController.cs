using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TarefasSite.Models;
using TarefasSite.Repositorio;
using TarefasSite.ViewModels;

namespace TarefasSite.Controllers
{
    public class TarefasController : Controller
    {
        private readonly IConfiguration _configuration;

        public TarefasController(IConfiguration configuration)
        {
            _configuration = configuration;         
        }

        [HttpGet]
        public IActionResult Index()
        {
            TarefaRepositorio repositorio = new TarefaRepositorio(_configuration);

            //buscar os registros da tabela de tarefas no repositorio
            List<Tarefa> tarefas = repositorio.Buscar();

            //Lista de Categorias
            CategoriaRepositorio categoriaRepositorio = new CategoriaRepositorio(_configuration);
            List<Categoria> categorias = categoriaRepositorio.Buscar();

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
                TarefaRepositorio repositorio = new TarefaRepositorio(_configuration);

                DateTime dataHora = new DateTime(tarefaViewModel.Data.Year, tarefaViewModel.Data.Month, tarefaViewModel.Data.Day, tarefaViewModel.Hora.Hour, tarefaViewModel.Hora.Minute, 0);

                Tarefa tarefa = new Tarefa(dataHora, tarefaViewModel.Descricao, tarefaViewModel.Notificacao, tarefaViewModel.IdCategoria.Value);

                repositorio.Inserir(tarefa);

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
            TarefaRepositorio repositorio = new TarefaRepositorio(_configuration);

            DateTime dataHora = new DateTime(tarefaViewModel.Data.Year, tarefaViewModel.Data.Month, tarefaViewModel.Data.Day, tarefaViewModel.Hora.Hour, tarefaViewModel.Hora.Minute, 0);

            Tarefa tarefa = new Tarefa(tarefaViewModel.Id, dataHora, tarefaViewModel.Descricao, tarefaViewModel.Notificacao, tarefaViewModel.IdCategoria.Value);

            repositorio.Atualizar(tarefa);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Deletar(int id)
        {
            TarefaRepositorio repositorio = new TarefaRepositorio(_configuration);

            repositorio.Excluir(id);

            return RedirectToAction("Index");
        }

        public IActionResult Detalhes(int id)
        {
            return BuscarTarefa(id);
        }

        private IActionResult BuscarTarefa(int id)
        {
            //buscar tarefa no repositorio
            TarefaRepositorio repositorio = new TarefaRepositorio(_configuration);

            Tarefa tarefa = repositorio.Buscar(id);

            TarefaViewModel tarefaViewModel = new TarefaViewModel();

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
            CategoriaRepositorio categoriaRepositorio = new CategoriaRepositorio(_configuration);
            List<Categoria> categorias = categoriaRepositorio.Buscar();

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

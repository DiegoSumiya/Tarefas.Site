using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tarefas.Dominio.Models;

namespace TarefasSite.ViewModels
{
    public class TarefaViewModel
    {
        public int Id { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public DateTime Hora { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatório")]
        [MinLength(5, ErrorMessage = "Descrição precisa ter mais de 5 caracteres")]
        public string Descricao { get; set; }

        public bool Notificacao { get; set; }

        [Required(ErrorMessage = "Categoria é obrigatório")]
        public int? IdCategoria { get; set; }
       
        public List<SelectListItem> Categorias { get; set; }
        public List<Usuario> Convidados { get; set; }
    }
}

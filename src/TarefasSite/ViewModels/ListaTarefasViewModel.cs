using System;

namespace TarefasSite.ViewModels
{
    public class ListaTarefasViewModel
    {
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Descricao { get; set; }

        public bool Notificacao { get; set; }

        public string Categoria { get; set; }

    }
}

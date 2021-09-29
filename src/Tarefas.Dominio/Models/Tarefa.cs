using System;

namespace Tarefas.Dominio.Models
{
    public class Tarefa
    {
        public Tarefa(DateTime data, string descricao, bool notificacao, int idCategoria, string idUsuario)
        {
            Data = data;
            Descricao = descricao;
            Notificacao = notificacao;
            IdCategoria = idCategoria;
            IdUsuario = idUsuario;
        }

        public Tarefa(int id, DateTime data, string descricao, bool notificacao, int idCategoria, string idUsuario)
        {
            Id = id;
            Data = data;
            Descricao = descricao;
            Notificacao = notificacao;
            IdCategoria = idCategoria;
            IdUsuario = idUsuario;

        }

        public int Id { get; set; }

        public DateTime Data { get; set; }
   
        public string Descricao { get; set; }

        public bool Notificacao { get; set; }

        public int IdCategoria { get; set; }
        public string IdUsuario { get; set; }

    }
}

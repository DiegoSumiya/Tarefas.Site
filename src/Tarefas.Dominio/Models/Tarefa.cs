using System;

namespace Tarefas.Dominio.Models
{
    public class Tarefa
    {
        public Tarefa(DateTime data, string descricao, bool notificacao, int idCategoria)
        {
            Data = data;
            Descricao = descricao;
            Notificacao = notificacao;
            IdCategoria = idCategoria;
        }

        public Tarefa(int id, DateTime data, string descricao, bool notificacao, int idCategoria)
        {
            Id = id;
            Data = data;
            Descricao = descricao;
            Notificacao = notificacao;
            IdCategoria = idCategoria;
        }

        public int Id { get; set; }

        public DateTime Data { get; set; }
   
        public string Descricao { get; set; }

        public bool Notificacao { get; set; }

        public int IdCategoria { get; set; }
        
    }
}

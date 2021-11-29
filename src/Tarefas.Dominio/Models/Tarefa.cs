using System;
using System.Collections.Generic;

namespace Tarefas.Dominio.Models
{
    public class Tarefa
    {
        public Tarefa(DateTime data, string descricao, bool notificacao, int idCategoria, string emailUsuario)
        {
            Data = data;
            Descricao = descricao;
            Notificacao = notificacao;
            IdCategoria = idCategoria;
            EmailUsuario = emailUsuario;
        }

        public Tarefa(int id, DateTime data, string descricao, bool notificacao, int idCategoria, string emailUsuario)
        {
            Id = id;
            Data = data;
            Descricao = descricao;
            Notificacao = notificacao;
            IdCategoria = idCategoria;
            EmailUsuario = emailUsuario;

        }




        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Descricao { get; set; }

        public bool Notificacao { get; set; }

        public int IdCategoria { get; set; }
        public string EmailUsuario { get; set; }
        public List<string> Convidados { get; set; }

        
        
        


    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Dominio.Models;

namespace Tarefas.Infra.Repositorio
{
    public interface ITarefaRepositorio
    {
        List<Tarefa> Buscar(string email);
        Tarefa Buscar(int id, string email);
        void Inserir(Tarefa tarefa);
        void Atualizar(Tarefa tarefa, string email);
        void Excluir(int id, string email);
    }
}

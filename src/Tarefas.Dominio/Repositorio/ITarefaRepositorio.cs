using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Dominio.Models;

namespace Tarefas.Infra.Repositorio
{
    public interface ITarefaRepositorio
    {
        List<Tarefa> Buscar();
        Tarefa Buscar(int id);
        void Inserir(Tarefa tarefa);
        void Atualizar(Tarefa tarefa);
        void Excluir(int id);
    }
}

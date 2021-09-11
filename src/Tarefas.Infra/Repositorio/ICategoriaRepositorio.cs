using System;
using System.Collections.Generic;
using System.Text;
using Tarefas.Dominio.Models;

namespace Tarefas.Infra.Repositorio
{
    public interface ICategoriaRepositorio
    {
        List<Categoria> Buscar();
    }
}

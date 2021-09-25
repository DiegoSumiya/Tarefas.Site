using System.Collections.Generic;
using Tarefas.Dominio.Models;

namespace Tarefas.Infra.Repositorio
{
    public interface ICategoriaRepositorio
    {
        List<Categoria> Buscar();
    }
}

using System.Collections.Generic;
using Tarefas.Dominio.Models;

namespace Tarefas.Dominio.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Usuario Buscar(string email);
        List<Usuario> Buscar();
        void Inserir(Usuario usuario);
    }
}

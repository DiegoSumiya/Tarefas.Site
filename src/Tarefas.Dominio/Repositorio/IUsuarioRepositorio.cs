using Tarefas.Dominio.Models;

namespace Tarefas.Dominio.Repositorio
{
    public interface IUsuarioRepositorio
    {
        Usuario Buscar(string email);
        void Inserir(Usuario usuario);
    }
}

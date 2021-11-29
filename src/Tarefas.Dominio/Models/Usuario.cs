using System.Collections.Generic;

namespace Tarefas.Dominio.Models
{
    public class Usuario
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
      

        public Usuario(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }

        public Usuario(string email)
        {
           
            Email = email;
        }
    }
}

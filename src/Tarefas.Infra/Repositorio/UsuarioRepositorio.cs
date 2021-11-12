using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using Tarefas.Dominio.Models;
using Tarefas.Dominio.Repositorio;

namespace Tarefas.Infra.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
       
        private readonly string _connectionString;

        public UsuarioRepositorio(IConfiguration configuration)
        {
            
            _connectionString = configuration.GetConnectionString("TarefasDB");
        }

        public Usuario Buscar(string email)
        {
            //buscar os registros da tabela de tarefas
            //1 Conectar no banco
            Usuario usuario = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //2 EXECUTAR COMANDO (SELECT)
                SqlCommand command = new SqlCommand("PR_TB_USUARIO_SELECT", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("email", email);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //3 LER INFORMACOES E ENVIAR PRA TELA
                if (reader.Read())
                {
                    string nome = reader["NOME"].ToString();
                    string senha = reader["SENHA"].ToString();


                    usuario = new Usuario(nome, email, senha);
                }

                connection.Close();
            }

            return usuario;
        }

        public void Inserir(Usuario usuario)
        {
            //Criar conexao com banco
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //Criar o comando
                SqlCommand command = new SqlCommand("PR_TB_USUARIO_INSERT", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("EMAIL", usuario.Email);
                command.Parameters.AddWithValue("NOME", usuario.Nome);
                command.Parameters.AddWithValue("SENHA", usuario.Senha);

                //Abrir conexao
                connection.Open();

                //Executar o comando
                command.ExecuteNonQuery();
            }

        }

        public List<Usuario> Buscar()
        {
            //buscar os registros da tabela de Usuario
            //1 Conectar no banco
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //2 EXECUTAR COMANDO (SELECT)
                SqlCommand command = new SqlCommand("PR_TB_USUARIO_LISTA_SELECT", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //3 LER INFORMACOES E ENVIAR PRA TELA
                while (reader.Read())
                {

                   
                    string email = reader["EMAIL"].ToString();
                    
                    Usuario item = new Usuario(email);

                    usuarios.Add(item);
                }

                connection.Close();
            }

            return usuarios;

        }
    }
}

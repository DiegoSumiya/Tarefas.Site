using Microsoft.Extensions.Configuration;
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
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"SELECT [NOME]
                      ,[EMAIL]
                      ,[SENHA]
                  FROM[dbo].[USUARIO]
                  WHERE EMAIL = @email";

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
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText =
                    @"INSERT INTO [dbo].[Usuario]
                            ([NOME]
                            ,[EMAIL]
                            ,[SENHA])
                           VALUES
                            (@NOME
                            , @EMAIL
                            , @SENHA)";

                command.Parameters.AddWithValue("NOME", usuario.Nome);
                command.Parameters.AddWithValue("EMAIL", usuario.Senha);
                command.Parameters.AddWithValue("SENHA", usuario.Email);

                //Abrir conexao
                connection.Open();

                //Executar o comando
                command.ExecuteNonQuery();
            }


        
        }   
    }
}

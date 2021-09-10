using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TarefasSite.Models;

namespace TarefasSite.Repositorio
{
    public class CategoriaRepositorio
    {
        private readonly string _connectionString;

        public CategoriaRepositorio(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TarefasDB");
        }

        public List<Categoria> Buscar()
        {
            List<Categoria> categorias = new List<Categoria>();

            //1- Criar uma conexao com o banco de dados
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //2- Criar um comando para executar no banco de dados
                SqlCommand command = new SqlCommand();
                command.CommandText = "SELECT [ID],[DESCRICAO] FROM [dbo].[TB_CATEGORIAS]";
                command.Connection = connection;

                //2.1 Abrir conexao
                connection.Open();

                //3- Executar o comando 
                SqlDataReader reader = command.ExecuteReader();

                //Ler o retorno
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string descricao = reader["DESCRICAO"].ToString();

                    Categoria categoria = new Categoria(id, descricao);

                    categorias.Add(categoria);
                }

                //3.1- Fechar conexao
                connection.Close();
            }

            return categorias;
        }
    }
}

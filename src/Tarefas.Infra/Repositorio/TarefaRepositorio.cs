using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Tarefas.Dominio.Models;

namespace Tarefas.Infra.Repositorio
{
    public class TarefaRepositorio
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TarefaRepositorio(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("TarefasDB");
        }

        public List<Tarefa> Buscar()
        {
            //buscar os registros da tabela de tarefas
            //1 Conectar no banco
            List<Tarefa> tarefas = new List<Tarefa>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //2 EXECUTAR COMANDO (SELECT)
                SqlCommand command = new SqlCommand(
                @"SELECT [ID]
                      ,[DATA]
                      ,[DESCRICAO]
                      ,[NOTIFICACAO]
                      ,[IDCATEGORIA]
                  FROM[dbo].[TAREFA]", connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //3 LER INFORMACOES E ENVIAR PRA TELA
                while (reader.Read())
                {
                    
                    int id = Convert.ToInt32(reader["ID"]);
                    DateTime data = Convert.ToDateTime(reader["DATA"]);
                    string descricao = reader["DESCRICAO"].ToString();
                    bool notificacao = Convert.ToBoolean(reader["NOTIFICACAO"]);
                    int idCategoria = Convert.ToInt32(reader["IDCATEGORIA"]);

                    Tarefa item = new Tarefa(id, data, descricao, notificacao, idCategoria);

                    tarefas.Add(item);
                }

                connection.Close();
            }

            return tarefas;

        }

        public Tarefa Buscar(int id)
        {
            //buscar os registros da tabela de tarefas
            //1 Conectar no banco
            Tarefa tarefa = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //2 EXECUTAR COMANDO (SELECT)
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"SELECT [ID]
                      ,[DATA]
                      ,[DESCRICAO]
                      ,[NOTIFICACAO]
                      ,[IDCATEGORIA]
                  FROM[dbo].[TAREFA]
                  WHERE ID = @id";

                command.Parameters.AddWithValue("id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //3 LER INFORMACOES E ENVIAR PRA TELA
                if (reader.Read())
                {
                    DateTime data = Convert.ToDateTime(reader["DATA"]);
                    string descricao = reader["DESCRICAO"].ToString();
                    bool notificacao = Convert.ToBoolean(reader["NOTIFICACAO"]);
                    int idCategoria = Convert.ToInt32(reader["IDCATEGORIA"]);

                    tarefa = new Tarefa(id, data, descricao, notificacao, idCategoria);
                }

                connection.Close();
            }

            return tarefa;
        }

        public void Inserir(Tarefa tarefa)
        {
            //Criar conexao com banco
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //Criar o comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText =
                    @"INSERT INTO [dbo].[TAREFA]
                            ([DATA]
                            ,[DESCRICAO]
                            ,[NOTIFICACAO]
                            ,[IDCATEGORIA])
                        VALUES
                            (@DATA
                            , @DESCRICAO
                            , @NOTIFICACAO
                            , @IDCATEGORIA)";

                command.Parameters.AddWithValue("DATA", tarefa.Data);
                command.Parameters.Add("DESCRICAO", System.Data.SqlDbType.VarChar);
                command.Parameters["DESCRICAO"].Value = tarefa.Descricao;
                command.Parameters.Add(new SqlParameter("NOTIFICACAO", tarefa.Notificacao));
                command.Parameters.Add(new SqlParameter("IDCATEGORIA", tarefa.IdCategoria));

                //Abrir conexao
                connection.Open();

                //Executar o comando
                command.ExecuteNonQuery();
            }

        }

        public void Atualizar(Tarefa tarefa)
        {
            //Criar conexao com banco
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //Criar o comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText =
                    @"UPDATE [dbo].[TAREFA]
                       SET [DATA] = @DATA
                          ,[DESCRICAO] = @DESCRICAO
                          ,[NOTIFICACAO] = @NOTIFICACAO
                          ,[IDCATEGORIA] = @IDCATEGORIA
                     WHERE ID = @ID";

                command.Parameters.AddWithValue("DATA", tarefa.Data);

                command.Parameters.Add("DESCRICAO", System.Data.SqlDbType.VarChar);
                command.Parameters["DESCRICAO"].Value = tarefa.Descricao;

                command.Parameters.Add(new SqlParameter("NOTIFICACAO", tarefa.Notificacao));

                command.Parameters.AddWithValue("ID", tarefa.Id);

                command.Parameters.Add(new SqlParameter("IDCATEGORIA", tarefa.IdCategoria));

                //Abrir conexao
                connection.Open();

                //Executar o comando
                command.ExecuteNonQuery();
            }
        }

        public void Excluir(int id)
        {
            //Criar conexao com banco
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //Criar o comando
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText =
                    @"DELETE FROM TAREFA WHERE ID = @ID";

                command.Parameters.AddWithValue("ID", id);

                //Abrir conexao
                connection.Open();

                //Executar o comando
                command.ExecuteNonQuery();
            }
        }
    }
}

using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Tarefas.Dominio.Models;

namespace Tarefas.Infra.Repositorio
{
    public class TarefaRepositorio : ITarefaRepositorio
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TarefaRepositorio(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("TarefasDB");
        }

        public List<Tarefa> Buscar(string email)
        {
            //buscar os registros da tabela de tarefas
            //1 Conectar no banco
            List<Tarefa> tarefas = new List<Tarefa>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //2 EXECUTAR COMANDO (SELECT)
                SqlCommand command = new SqlCommand("PR_TB_TAREFA_LISTA_SELECT", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("email", email);

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
                    string emailUsuario = reader["EMAIL_USUARIO"].ToString();

                    Tarefa item = new Tarefa(id, data, descricao, notificacao, idCategoria, emailUsuario);

                    tarefas.Add(item);
                }
                
                connection.Close();
            }

            return tarefas;

        }

        public Tarefa Buscar(int id, string email)
        {
            //buscar os registros da tabela de tarefas
            //1 Conectar no banco
            Tarefa tarefa = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //2 EXECUTAR COMANDO (SELECT)
                SqlCommand command = new SqlCommand("PR_TB_TAREFA_ID_SELECT", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("email", email);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //3 LER INFORMACOES E ENVIAR PRA TELA
                if (reader.Read())
                {
                    DateTime data = Convert.ToDateTime(reader["DATA"]);
                    string descricao = reader["DESCRICAO"].ToString();
                    bool notificacao = Convert.ToBoolean(reader["NOTIFICACAO"]);
                    int idCategoria = Convert.ToInt32(reader["IDCATEGORIA"]);
                    string emailUsuario = reader["EMAIL_USUARIO"].ToString();

                    tarefa = new Tarefa(id, data, descricao, notificacao, idCategoria, emailUsuario);
                }
                connection.Close();


                SqlCommand command1 = new SqlCommand("PR_TB_CONVIDADO_ID_SELECT", connection);
                command1.CommandType = System.Data.CommandType.StoredProcedure;

                command1.Parameters.AddWithValue("id", id);
                connection.Open();
                SqlDataReader reader1 = command1.ExecuteReader();

                while(reader1.Read())
                {
                    string convidados = reader1["EMAIL_CONVIDADO"].ToString();
                    tarefa.Convidados = new List<string>();
                    tarefa.Convidados.Add(convidados);
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
                SqlCommand command = new SqlCommand("PR_TB_TAREFA_INSERT", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                
                command.Parameters.AddWithValue("DATA", tarefa.Data);
                command.Parameters.Add("DESCRICAO", System.Data.SqlDbType.VarChar);
                command.Parameters["DESCRICAO"].Value = tarefa.Descricao;
                command.Parameters.Add(new SqlParameter("NOTIFICACAO", tarefa.Notificacao));
                command.Parameters.Add(new SqlParameter("IDCATEGORIA", tarefa.IdCategoria));
                command.Parameters.Add(new SqlParameter("EMAIL_USUARIO", tarefa.EmailUsuario));
               
                //Abrir conexao
                connection.Open();
                //Executar o comando
                int Id = Convert.ToInt32(command.ExecuteScalar());

                if(tarefa.Convidados != null)
                {
                    foreach (var item in tarefa.Convidados)
                    {
                        SqlCommand command1 = new SqlCommand("PR_TB_CONVIDADO_TAREFA_INSERT", connection);
                        command1.CommandType = System.Data.CommandType.StoredProcedure;

                        command1.Parameters.AddWithValue("EMAIL_CONVIDADO", item);
                        command1.Parameters.AddWithValue("ID_TAREFA", Id);
                        command1.ExecuteNonQuery();
                    }
                    

                }
                
                
            }

        }

        public void Atualizar(Tarefa tarefa, string email)
        {
            //Criar conexao com banco
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //Criar o comando
                SqlCommand command = new SqlCommand("PR_TB_TAREFA_UPDATE", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("DATA", tarefa.Data);
                command.Parameters.AddWithValue("email", email);


                command.Parameters.Add("DESCRICAO", System.Data.SqlDbType.VarChar);
                command.Parameters["DESCRICAO"].Value = tarefa.Descricao;

                command.Parameters.Add(new SqlParameter("NOTIFICACAO", tarefa.Notificacao));

                command.Parameters.AddWithValue("ID", tarefa.Id);

                command.Parameters.Add(new SqlParameter("IDCATEGORIA", tarefa.IdCategoria));

                //Abrir conexao
                connection.Open();
                //Executar o comando
                command.ExecuteNonQuery();
                if (tarefa.Convidados != null)
                {
                    SqlCommand command2 = new SqlCommand("PR_TB_CONVIDADO_DELETE", connection);
                    command2.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    command2.Parameters.AddWithValue("ID", tarefa.Id);
                    command2.ExecuteNonQuery();

                    foreach (var item in tarefa.Convidados)
                    {
                        SqlCommand command3 = new SqlCommand("PR_TB_CONVIDADO_TAREFA_INSERT", connection);
                        command3.CommandType = System.Data.CommandType.StoredProcedure;

                        command3.Parameters.AddWithValue("EMAIL_CONVIDADO", item);
                        command3.Parameters.AddWithValue("ID_TAREFA", tarefa.Id);
                        command3.ExecuteNonQuery();
                    }


                }

                
            }
        }

        public void Excluir(int id, string email)
        {
            //Criar conexao com banco
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //Criar o comando
                SqlCommand command = new SqlCommand("PR_TB_TAREFA_DELETE", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("ID", id);
                command.Parameters.AddWithValue("email", email);

                //Abrir conexao
                connection.Open();

                //Executar o comando
                command.ExecuteNonQuery();
            }
        }
    }
}

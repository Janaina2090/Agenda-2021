using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Web.Agenda.Models;

namespace Web.Agenda.Data
{
    public class Repository : IRepository
    {
        SqlConnection conn = null;
        SqlCommand comando = null;
        bool result = false;

        public Repository()
        {
            if (conn == null)
            {
                var stringConexao = ConfigurationManager.ConnectionStrings["ConexaoPessoaDB"].ConnectionString;

                conn = new SqlConnection(stringConexao);
            }
            if (comando == null)
            {
                comando = new SqlCommand();
            }
        }
        public bool Create(TarefaMod tarefa)
        {
            try
            {
                comando.CommandType = CommandType.Text;
                comando.CommandText = @"INSERT INTO TAREFAS( Nome, Prioridade, Concluido, Observacao) VALUES(" + "@Nome,@Prioridade,@Concluido,@Observacao)";

                comando.Parameters.AddWithValue("@Nome", tarefa.Nome.Trim().ToUpper());
                comando.Parameters.AddWithValue("@Prioridade", tarefa.Prioridade);
                comando.Parameters.AddWithValue("@Concluido", tarefa.Concluido);
                comando.Parameters.AddWithValue("@Observacao", tarefa.Observacao);
                comando.Connection = conn;
                conn.Open();
                result = comando.ExecuteNonQuery() >=1 ? true : false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return result;
        }

        public bool Delete(int id)
        {
            int total = 0;
            var sql = @"DELETE TAREFAS WHERE ID=@id";
            using (var conexao =  new SqlConnection(ConfigurationManager.ConnectionStrings["ConexaoPessoaDB"].ConnectionString))
            {
                using (var comando = new SqlCommand()) 
                {
                    comando.Connection = conexao;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = sql;

                    comando.Parameters.AddWithValue("@id", id);

                    conexao.Open();
                    total = Convert.ToInt32(comando.ExecuteNonQuery());
                }
            }
            return total > 0 ? true : false;
        }

        public TarefaMod Read(int Id)
        {
            TarefaMod tarefa = null;
            try
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexaoPessoaDB"].ConnectionString))
                {
                    comando.Connection = con;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = @"SELECT Id, Nome, Prioridade, Concluido,Observacao FROM TAREFAS WHERE Id=@Id";
                    comando.Parameters.AddWithValue("@Id", Id);
                    con.Open();
                    using (var dr = comando.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            tarefa = new TarefaMod()
                            {
                                Id = Convert.ToInt32(dr["id"]),
                                Nome = Convert.ToString(dr["Nome"]),
                                Prioridade = Convert.ToInt32(dr["Prioridade"]),
                                Concluido = Convert.ToBoolean(dr["Concluido"]),
                                Observacao = Convert.ToString(dr["Observacao"])
                            };
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return tarefa;
        }

        public List<TarefaMod> ReadAll()
        {
            List<TarefaMod> lista = new List<TarefaMod>();

            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexaoPessoaDB"].ConnectionString))
                {
                    comando.Connection = conn;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = @"SELECT *FROM TAREFAS ORDER BY PRIORIDADE";

                    conn.Open();

                    using (var dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var tarefa = new TarefaMod();
                            tarefa.Id = Convert.ToInt32(dr["id"]);
                            tarefa.Nome = Convert.ToString(dr["nome"]);
                            tarefa.Prioridade = Convert.ToInt32(dr["prioridade"]);
                            tarefa.Concluido = Convert.ToBoolean(dr["Concluido"]);
                            tarefa.Observacao = Convert.ToString(dr["Observacao"]);

                            lista.Add(tarefa);
                        };
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return lista;
        }

        public bool Update(TarefaMod tarefa)
        {
            int total = 0;
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexaoPessoaDB"].ConnectionString))
                {
                    comando.Connection = conn;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = @"UPDATE TAREFAS SET Nome=@Nome,Prioridade=@Prioridade,Concluido=@Concluido, Observacao=@Observacao WHERE Id=@Id";

                    comando.Parameters.AddWithValue("Nome", tarefa.Nome);
                    comando.Parameters.AddWithValue("Prioridade", tarefa.Prioridade);
                    comando.Parameters.AddWithValue("Concluido", tarefa.Concluido);
                    comando.Parameters.AddWithValue("Observacao", tarefa.Observacao);
                    comando.Parameters.AddWithValue("Id", tarefa.Id);

                    conn.Open();

                    total = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
            return total > 0 ?true: false;

        }
    }
}
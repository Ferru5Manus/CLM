using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace SchoolDatabaseRepository
{
    public class ResultsDatabaseRepository : IResultRepository
    {
        public void AddResult(ResultDto result)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {

                string sqlQuery1 = "UPDATE results SET taskResult = @taskResult WHERE id LIKE @id; ";

                int rowsAffected = db.Execute(sqlQuery1, result);
            }
        }

        public void CreateResultTemplates(ResultDto result)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {
                string sqlQuery1= "INSERT INTO results (login,taskGrId,taskId,formString,taskResult) Values(@login,@taskGrId,@taskId,@formString,@taskResult)";
                int rowsAffected = db.Execute(sqlQuery1, result);
            }
        }

        public List<string> GetAllResults()
        {
            List<string> lst = new List<string>();
            

            return lst;
        }

        public List<string> GetIdsByTask(ResultDto results)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<ResultDto>("SELECT * FROM results where taskGrId=@taskGrId and taskId=@taskId and formString=@formString ", new { results.taskGrId, results.taskId , results.formString}).ToList();
                lst = result.Select(results => results.id.ToString()).ToList();
            }

            return lst;
        }

        public List<string> GetIdsByTaskLogin(ResultDto results)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<ResultDto>("SELECT * FROM results where taskGrId=@taskGrId and taskId=@taskId and login=@login and formString=@formString", new { results.taskGrId, results.taskId ,results.login, results.formString}).ToList();
                lst = result.Select(results => results.id.ToString()).ToList();
            }

            return lst;
        }

        public List<string> GetResultByLogin(ResultDto results)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<ResultDto>("SELECT * FROM results where login = @login and taskGrId=@taskGrId and taskId=@taskId and formString=@formString", new { results.login,results.taskGrId,results.taskId, results.formString }).ToList();
                lst = result.Select(results => results.taskResult.ToString()).ToList();
            }

            return lst;
        }

        

        public void RemoveResult(ResultDto result)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {
                string sqlQuery1 = "DELETE FROM results where id=@id";

                int rowsAffected = db.Execute(sqlQuery1, result);
            }
        }

        public void RemoveResultByForm(ResultDto result)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {
                string sqlQuery1 = "DELETE FROM results where formString=@formString";

                int rowsAffected = db.Execute(sqlQuery1, result);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace SimpleWebApp.Repository
{
	public class NewsDatabaseRepository : INewsRepository
	{
		public void SaveNew(NewsDto news)
		{
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {
                string sqlQuery1 = "INSERT INTO news (TitleString,TextString) Values(@TitleString,@TextString)";

                int rowsAffected = db.Execute(sqlQuery1, news);
            }   
        }

        

        public List<string> GetAllTitles()
        {
            List<string> lst = new List<string>();
            //The object that will physically connect to the database
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<NewsDto>("SELECT * FROM news").ToList();
                lst = result.Select(news => news.TitleString).ToList();
            }

            return lst;
        }

        public List<string> GetAllNewsText()
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<NewsDto>("SELECT * FROM news").ToList();
                lst = result.Select(news => news.TextString).ToList();
            }

            return lst;
        }

        public List<string> GetAllIds()
        {
             List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<NewsDto>("SELECT * FROM news").ToList();
                lst = result.Select(news => news.Id.ToString()).ToList();
            }

            return lst;
        }

        public void RemoveNew(NewsDto news)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {
                string sqlQuery1 = "DELETE FROM news where id=@Id";

                int rowsAffected = db.Execute(sqlQuery1, news);
            }
        }

        public void ChangeNewTitle(NewsDto news)
        {
                using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
                {

                    string sqlQuery1 = "UPDATE news SET TitleString = @newTitleString WHERE id LIKE @id; ";

                    int rowsAffected = db.Execute(sqlQuery1, news);
                }
            
            
        }
        public void ChangeNewText(NewsDto news)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {
                string sqlQuery1 = "UPDATE news SET TextString = @newTextString WHERE id LIKE @id; ";

                int rowsAffected = db.Execute(sqlQuery1, news);
            }
        }
    }
}

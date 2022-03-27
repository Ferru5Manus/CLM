using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;

namespace SchoolDatabaseRepository
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

       

        public string GetTextById(NewsDto news)
        {
            List<string> newsTextes = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {

                var result = cnx.Query<NewsDto>("select * from news where id=@id", new { news.Id }).ToList();
                newsTextes = result.Select(text => text.TextString.ToString()).ToList();
                return newsTextes[0];



            }
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

        public string GetTitleById(NewsDto news)
        {
            List<string> newsTitles = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                
                    var result = cnx.Query<NewsDto>("select * from news where id=@id", new { news.Id }).ToList();
                    newsTitles = result.Select(titles => titles.TitleString.ToString()).ToList();
                    return newsTitles[0];
                
                

            }
        }

        public string GetNewsIdByTitle(NewsDto news)
        {
            List<string> newsIds = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {

                var result = cnx.Query<NewsDto>("select * from news where title=@title", new { news.TitleString }).ToList();
                newsIds = result.Select(ids => ids.Id.ToString()).ToList();
                return newsIds[0];
            }
        }
    }
}

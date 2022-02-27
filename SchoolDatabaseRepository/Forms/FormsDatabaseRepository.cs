using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabaseRepository
{
    public class FormsDatabaseRepository : IFormsRepository
    {
       
        

     

        public List<string> GetAllForms()
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<FormDto>("SELECT * FROM forms").ToList();
                lst = result.Select(forms => forms.FormString).ToList();
            }

            return lst;
        }

        public List<string> GetAllIds()
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<FormDto>("SELECT * FROM forms").ToList();
                lst = result.Select(forms => forms.Id.ToString()).ToList();
            }

            return lst;
        }
        public List<string> GetForms(FormDto forms)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<FormDto>("SELECT * FROM forms where Id = @Id", new { forms.Id }).ToList();
                lst = result.Select(forms => forms.FormString.ToString()).ToList();
            }

            return lst;
        }

        public void RemoveForm(FormDto forms)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {
                string sqlQuery1 = "DELETE FROM forms where id=@Id";

                int rowsAffected = db.Execute(sqlQuery1, forms);
            }
        }

        public void SaveForms(FormDto news)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {
                string sqlQuery1 = "INSERT INTO forms (FormString) Values(@FormString)";

                int rowsAffected = db.Execute(sqlQuery1, news);
            }
        }
    }
}

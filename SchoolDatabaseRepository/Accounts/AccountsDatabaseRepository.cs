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
    public class AccountsDatabaseRepository : IAccountsRepository
    {
        public void AddAccount(AccountsDto account)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {
                string sqlQuery1 = "INSERT INTO users (login,email,password,role,form) Values(@login,@email,@password,@role,@form)";

                int rowsAffected = db.Execute(sqlQuery1, account);
            }
        }
        public List<string> GetAllUsersIds()
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<AccountsDto>("SELECT * FROM users").ToList();
                lst = result.Select(accounts => accounts.id.ToString()).ToList();
            }

            return lst;
        }

        public List<string> GetPasswordByEmail(AccountsDto accounts)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<AccountsDto>("select * from users where email = @email", new { accounts.email }).ToList();
                lst = result.Select(account => account.password.ToString()).ToList();
            }


            return lst;
        }
        public List<string> GetPasswordByLogin(AccountsDto accounts)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<AccountsDto>("select * from users where login = @login", new { accounts.login }).ToList();
                lst = result.Select(account => account.password.ToString()).ToList();
            }


            return lst;
        }
        public List<string> GetRoleByLogin(AccountsDto accounts)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<AccountsDto>("select * from users where login = @login", new { accounts.login }).ToList();
                lst = result.Select(account => account.role.ToString()).ToList();
            }


            return lst;
        }
        public List<string> GetUserByLogin(AccountsDto account)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                try
                {
                    var result = cnx.Query<AccountsDto>("select * from users where login = @login", new { account.login }).ToList();
                    lst = result.Select(account => account.id.ToString()).ToList();
                    return lst;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }



        }
        public List<string> GetUserByEmail(AccountsDto account)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                try
                {
                    var result = cnx.Query<AccountsDto>("select * from users where email = @email", new { account.email }).ToList();
                    lst = result.Select(account => account.id.ToString()).ToList();
                    return lst;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }



        }
        public List<string> GetLoginByEmail(AccountsDto accounts)
        {
            List<string> lst = new List<string>();
            using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
            {
                var result = cnx.Query<AccountsDto>("select * from users where email = @email", new { accounts.email }).ToList();
                lst = result.Select(news => news.id.ToString()).ToList();
            }


            return lst;
        }
    public List<string> GetUserEmails()
    {
        List<string> lst = new List<string>();
        using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
        {
            var result = cnx.Query<AccountsDto>("SELECT * FROM users").ToList();
            lst = result.Select(accounts => accounts.email.ToString()).ToList();
        }

        return lst;
    }

    public List<string> GetUserForms()
    {
        List<string> lst = new List<string>();
        using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
        {
            var result = cnx.Query<AccountsDto>("SELECT * FROM users").ToList();
            lst = result.Select(accounts => accounts.form.ToString()).ToList();
        }

        return lst;
    }

    public List<string> GetUserLogins()
    {
        List<string> lst = new List<string>();
        using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
        {
            var result = cnx.Query<AccountsDto>("SELECT * FROM users").ToList();
            lst = result.Select(accounts => accounts.login.ToString()).ToList();
        }

        return lst;
    }

    public List<string> GetUserRoles()
    {
        List<string> lst = new List<string>();
        using (MySqlConnection cnx = new MySqlConnection("Server = 127.0.0.1; Database = clm; Uid = root; Pwd = root;"))
        {
            var result = cnx.Query<AccountsDto>("SELECT * FROM users").ToList();
            lst = result.Select(accounts => accounts.role.ToString()).ToList();
        }

        return lst;
    }

    public void UpdateEmail(AccountsDto account)
    {
        using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
        {

            string sqlQuery1 = "UPDATE users SET email = @email WHERE id LIKE @id; ";

            int rowsAffected = db.Execute(sqlQuery1, account);
        }
    }

    public void UpdateForm(AccountsDto account)
    {
        using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
        {

            string sqlQuery1 = "UPDATE users SET form = @form WHERE id LIKE @id; ";

            int rowsAffected = db.Execute(sqlQuery1, account);
        }
    }

    public void UpdateLogin(AccountsDto account)
    {
        using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
        {

            string sqlQuery1 = "UPDATE users SET login = @login WHERE id LIKE @id; ";

            int rowsAffected = db.Execute(sqlQuery1, account);
        }
    }

        public void UpdateRole(AccountsDto account)
        {
            using (IDbConnection db = new MySqlConnection("Server=127.0.0.1;Database=clm;Uid=root;Pwd=root;"))
            {

                string sqlQuery1 = "UPDATE users SET role = @role WHERE id LIKE @id; ";

                int rowsAffected = db.Execute(sqlQuery1, account);
            }
        }
    }
 }


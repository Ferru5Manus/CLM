using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace SchoolSite100._0
{
    public class AutentificationManager
    {
        List<User> users = new List<User>();

        

        public void Register(string login, string password, string email)
        {
            if (IsRegistred(login, password,email) == false)
            {
                users.Add(new User { id = users.Count() + 1, login = login, email = email, password = EncryptPlainTextToCipherText(password), role = "Student", form = "unknown" });
            }
        }
        public bool IsRegistred(string login, string password, string email)
        {
            bool flag = false;
            string s = EncryptPlainTextToCipherText(password);
            foreach (var item in users)
            {
                if (item.login == login && item.password == s && item.email==email)
                {
                    flag = true;
                }

            }
            return flag;


        }
        public bool IsRegistred(string login, string password)
        {
            bool flag = false;
            string s = EncryptPlainTextToCipherText(password);
            foreach (var item in users)
            {
                if (item.login == login && item.password == s)
                {
                    flag = true;
                }

            }
            return flag;


        }

        public bool IsExists(string email)
        {
            bool flag = false;

            foreach (var item in users)
            {
                if (item.email == email)
                {
                    flag = true;
                }

            }
            return flag;


        }
        public bool IsAdmin(string login)
        {
            foreach (var item in users)
            {
                if (item.login == login)
                {
                    if (item.role=="Admin")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
       

        public List<string> GetUserIds()
        {
            List<string> i = new List<string>();
            for (int z = 0; z < users.Count(); z++)
            {
                i.Add(users[z].id.ToString());
            }
            return i;
        }
        public List<string> GetUserLogins()
        {
            List<string> i = new List<string>();
            for (int z = 0; z < users.Count(); z++)
            {
                i.Add(users[z].login.ToString());
            }
            return i;
        }
        public List<string> GetUserEmails()
        {
            List<string> i = new List<string>();
            for (int z = 0; z < users.Count(); z++)
            {
                i.Add(users[z].email.ToString());
            }
            return i;
        }
        public List<string> GetUserRoles()
        {
            List<string> i = new List<string>();
            for (int z = 0; z < users.Count(); z++)
            {
                i.Add(users[z].role.ToString());
            }
            return i;
        }
        public List<string> GetUserForms()
        {
            List<string> i = new List<string>();
            for (int z = 0; z < users.Count(); z++)
            {
                i.Add(users[z].form.ToString());
            }
            return i;
        }

        public void UpdateLogin(int id, string login)
        {
            users[id - 1].login = login;
        }
        public void UpdateEmail(int id, string email)
        {
            users[id - 1].email = email;
        }
        public void UpdateForm(int id, string form)
        {
            users[id - 1].form = form;
        }
        public void UpdateRole(int id, string role)
        {
            users[id - 1].role = role;
        }
        public void SendPassword(string email)
        {
            MailAddress from = new MailAddress("cmspassreq@gmail.com", "Lms");
            string login = "";
            string password = "";
            foreach (var item in users)
            {
                if (item.email == email)
                {
                    login = item.login;
                    password = DecryptCipherTextToPlainText(item.password);
                }
            }
            // кому отправляем
            MailAddress to = new MailAddress(email);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Восстановление парля";
            // текст письма
            m.Body = "<h2>Здравствуйте, недавно вы пытались восстановить данные своего пароля.</h2><span>Ваш логин: "+login+" Ваш пароль:"+password+"</span>";

            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("cmspassreq@gmail.com", "1234ww1234ww");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
        private const string SecurityKey = "NeroClaudiusIsBest22899321";

        //This method is used to convert the plain text to Encrypted/Un-Readable Text format.
        public static string EncryptPlainTextToCipherText(string PlainText)
        {
            // Getting the bytes of Input String.
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            //De-allocatinng the memory after doing the Job.
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;


            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        //This method is used to convert the Encrypted/Un-Readable Text back to readable  format.
        public static string DecryptCipherTextToPlainText(string CipherText)
        {
            byte[] toEncryptArray = Convert.FromBase64String(CipherText);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();

            //Convert and return the decrypted data/byte into string format.
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
       
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using SchoolDatabaseRepository;

namespace SchoolSite100._0
{
    public class AutentificationManager
    {
        private AccountsDatabaseRepository AccountDto = new AccountsDatabaseRepository();
        private FormsManager forms = new FormsManager();
        private List<string> roles = new List<string> { "Admin", "User", "Teacher" };
        public bool Register(string login, string password, string email)
        {
            if (IsRegistred(login,password,email)==false)
            {
                AccountDto.AddAccount(new AccountsDto { login=login,password=EncryptPlainTextToCipherText(password),email=email,form="",role="User" }) ;
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsRegistred(string login, string password, string email)
        {
            if (AccountDto.GetUserByEmail(new AccountsDto { email = email }).Count!=0|| AccountDto.GetPasswordByLogin(new AccountsDto { login = login }).Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IsRegistred(string login, string password)
        {
            if (AccountDto.GetUserByLogin(new AccountsDto { login = login }) != null && AccountDto.GetPasswordByLogin(new AccountsDto { login = login }) != null && AccountDto.GetPasswordByLogin(new AccountsDto { login = login }).Count() > 0 && AccountDto.GetPasswordByLogin(new AccountsDto { login = login })[0] == EncryptPlainTextToCipherText(password))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IsRegistredEarlier(string login,string email)
        {
            if (AccountDto.GetUserByLogin(new AccountsDto { login = login }).Count!=0 && AccountDto.GetPasswordByEmail(new AccountsDto { email = email}).Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool IsExists(string email)
        {
            if (AccountDto.GetUserByEmail(new AccountsDto {email=email}).Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetRoleByLogin(string login)
        {
            if(AccountDto.GetRoleByLogin(new AccountsDto { login = login }) != null)
            {
                
                return AccountDto.GetRoleByLogin(new AccountsDto { login = login })[0];
            }
            return "";
        }


        public List<string> GetUserIds()
        {
            List<string> i = new List<string>();
            i = AccountDto.GetAllUsersIds();
            return i;
        }
        public List<string> GetFormByLogin(string login)
        {
            List<string> i = new List<string>();
            i = AccountDto.GetFormByLogin(new AccountsDto { login = login });
            return i;
        }
        public List<string> GetUserLogins()
        {
            List<string> i = new List<string>();
            i = AccountDto.GetUserLogins();
            return i;
        }
        public List<string> GetUserEmails()
        {
            List<string> i = new List<string>();
            i = AccountDto.GetUserEmails();
            return i;
        }
        public List<string> GetUserRoles()
        {
            List<string> i = new List<string>();
            i = AccountDto.GetUserRoles();
            return i;
        }
        public List<string> GetUserForms()
        {
            List<string> i = new List<string>();
            i = AccountDto.GetUserForms();
            return i;
        }
        public string GetEmailByLogin(string login)
        {
            List<string> i = new List<string>();
            i = AccountDto.GetEmailByLogin(new AccountsDto() { login = login });
            return i[0];
        }
       
       
        public bool UpdateForm(int id, string form)
        {
            
            
            if (forms.IsFormExists(form))
            {
                AccountDto.UpdateForm(new AccountsDto { form = form, id = id });
                return true;
            }
            return false;
           
        }
        public bool UpdateRole(int id, string role)
        {
            foreach (var item in roles)
            {
                if (item==role)
                {
                    AccountDto.UpdateRole(new AccountsDto { role = role, id = id });
                    return true;
                }
            }
            return false;
        }
        public bool UpdateLogin(string login, string newLogin)
        {
            List<string> lst = AccountDto.GetUserLogins();
            foreach (var item in lst)
            {
                if (newLogin == item)
                {
                    return false;
                }
            }
            int id = Convert.ToInt32(AccountDto.GetUserByLogin(new AccountsDto() { login = login })[0]);
            AccountDto.UpdateLogin(new AccountsDto() { id = id, login = newLogin}) ;
            return true;
        }
        public bool UpdateEmail(string login, string email)
        {
            List<string> lst = AccountDto.GetUserEmails();
            foreach (var item in lst)
            {
                if (email == item)
                {
                    return false;
                }
            }
            int id = Convert.ToInt32(AccountDto.GetUserByLogin(new AccountsDto() { login = login })[0]);
            AccountDto.UpdateEmail(new AccountsDto() { id = id, email = email });
            return true;
        }
        public void UpdatePassword(string login, string password)
        {
            int id = Convert.ToInt32(AccountDto.GetUserByLogin(new AccountsDto() { login = login })[0]);
            AccountDto.UpdatePassword(new AccountsDto() { id = id, password=EncryptPlainTextToCipherText(password) });
        }
        private Random _random = new Random(Environment.TickCount);

        public string RandomString(int length)
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }
        public void SendPassword(string email)
        {
            MailAddress from = new MailAddress("cmspassreq@gmail.com", "Lms");
            string login = AccountDto.GetLoginByEmail(new AccountsDto { email = email })[0];
            string password = RandomString(8);
            UpdatePassword(login, password);
            //REDO
            
            // кому отправляем
            MailAddress to = new MailAddress(email);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Восстановление пароля";
            // текст письма
            m.Body = "<h2>Здравствуйте, недавно вы пытались восстановить данные своего аккаунта.</h2><span>Ваш логин: "+login+" Ваш пароль:"+password+"</span>";

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
      
    }
       
    
}

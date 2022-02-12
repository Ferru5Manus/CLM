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
        public void Register(string login, string password, string email)
        {
            if (IsRegistred(login,password,email)==false)
            {
                AccountDto.AddAccount(new AccountsDto { login=login,password=EncryptPlainTextToCipherText(password),email=email,form="",role="User" }) ;
            }
        }
        public bool IsRegistred(string login, string password, string email)
        {
            if (AccountDto.GetUserByEmail(new AccountsDto { email = email }) != null)
            {
                if (AccountDto.GetPasswordByLogin(new AccountsDto {login=login})!=null)
                {
                    if (AccountDto.GetPasswordByLogin(new AccountsDto { login = login }).Count() > 0)
                    {
                        if (AccountDto.GetPasswordByLogin(new AccountsDto { login = login })[0] == EncryptPlainTextToCipherText(password))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                    
                    
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return false;
            }

        }
        public bool IsRegistred(string login, string password)
        {
            if (AccountDto.GetUserByLogin(new AccountsDto { login = login }) != null )
            {
                if (AccountDto.GetPasswordByLogin(new AccountsDto { login = login }) != null)
                {
                    if (AccountDto.GetPasswordByLogin(new AccountsDto { login = login }).Count() > 0)
                    {
                        if (AccountDto.GetPasswordByLogin(new AccountsDto { login = login })[0] == EncryptPlainTextToCipherText(password))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
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
        public bool IsAdmin(string login)
        {
            if(AccountDto.GetRoleByLogin(new AccountsDto { login = login }) != null)
            {
                if (AccountDto.GetRoleByLogin(new AccountsDto { login = login })[0]=="Admin")
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        

        public List<string> GetUserIds()
        {
            List<string> i = new List<string>();
            i = AccountDto.GetAllUsersIds();
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

        public void UpdateLogin(int id, string login)
        {
            AccountDto.UpdateLogin(new AccountsDto { login = login, id = id });
        }
        public void UpdateEmail(int id, string email)
        {
            AccountDto.UpdateEmail(new AccountsDto { email = email, id = id });
        }
        public void UpdateForm(int id, string form)
        {
            AccountDto.UpdateForm(new AccountsDto { form=form, id = id });
        }
        public void UpdateRole(int id, string role)
        {
            AccountDto.UpdateRole (new AccountsDto { role = role, id = id });
        }
        public void SendPassword(string email)
        {
            MailAddress from = new MailAddress("cmspassreq@gmail.com", "Lms");
            string login = AccountDto.GetLoginByEmail(new AccountsDto { email = email })[0];
            string password = DecryptCipherTextToPlainText(AccountDto.GetPasswordByEmail(new AccountsDto { email = email })[0]);
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

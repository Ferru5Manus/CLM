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
        private AccountsDatabaseRepository m_AccountsRepository = new AccountsDatabaseRepository();
        private FormsManager m_FormsRepository = new FormsManager();
        private List<string> m_RolesList = new List<string> { "Admin", "User", "Teacher" };
        public void Register(string login, string password, string email)
        {
            if (IsRegistred(login,password,email)==false)
            {
                m_AccountsRepository.AddAccount(new AccountsDto { login=login,password=EncryptPlainTextToCipherText(password),email=email,form="",role="User" }) ;
            }
        }
        public bool IsRegistred(string login, string password, string email)
        {
            if (m_AccountsRepository.GetUserByEmail(new AccountsDto { email = email }) != null)
            {
                if (m_AccountsRepository.GetPasswordByLogin(new AccountsDto { login = login }).Count() > 0)
                {
                    if (m_AccountsRepository.GetPasswordByLogin(new AccountsDto { login = login })[0] == EncryptPlainTextToCipherText(password))
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
        public bool IsRegistred(string login, string password)
        {
            if (m_AccountsRepository.GetUserByLogin(new AccountsDto { login = login }) != null )
            {
                if (m_AccountsRepository.GetPasswordByLogin(new AccountsDto { login = login }) != null)
                {
                    if (m_AccountsRepository.GetPasswordByLogin(new AccountsDto { login = login }).Count() > 0)
                    {
                        if (m_AccountsRepository.GetPasswordByLogin(new AccountsDto { login = login })[0] == EncryptPlainTextToCipherText(password))
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
            if (m_AccountsRepository.GetUserByLogin(new AccountsDto { login = login }).Count!=0 && m_AccountsRepository.GetPasswordByEmail(new AccountsDto { email = email}).Count != 0)

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
            if (m_AccountsRepository.GetUserByEmail(new AccountsDto {email=email}).Count>0)
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
            if(m_AccountsRepository.GetRoleByLogin(new AccountsDto { login = login }) != null)
            {
                
                return m_AccountsRepository.GetRoleByLogin(new AccountsDto { login = login })[0];
            }
            return "";
        }


        public List<string> GetUserIds()
        {
            List<string> i = new List<string>();
            i = m_AccountsRepository.GetAllUsersIds();
            return i;
        }
        public List<string> GetFormByLogin(string login)
        {
            List<string> i = new List<string>();
            i = m_AccountsRepository.GetFormByLogin(new AccountsDto { login = login });
            return i;
        }
        public List<string> GetUserLogins()
        {
            List<string> i = new List<string>();
            i = m_AccountsRepository.GetUserLogins();
            return i;
        }
        public List<string> GetUserEmails()
        {
            List<string> i = new List<string>();
            i = m_AccountsRepository.GetUserEmails();
            return i;
        }
        public List<string> GetUserRoles()
        {
            List<string> i = new List<string>();
            i = m_AccountsRepository.GetUserRoles();
            return i;
        }
        public List<string> GetUserForms()
        {
            List<string> i = new List<string>();
            i = m_AccountsRepository.GetUserForms();
            return i;
        }
        public string GetEmailByLogin(string login)
        {
            List<string> i = new List<string>();
            i = m_AccountsRepository.GetEmailByLogin(new AccountsDto() { login = login });
            return i[0];
        }
       
       
        public bool UpdateForm(int id, string form)
        {
            
            
            if (m_FormsRepository.IsFormExists(form))
            {
                m_AccountsRepository.UpdateForm(new AccountsDto { form = form, id = id });
                return true;
            }
            return false;
           
        }
        public bool UpdateRole(int id, string role)
        {
            foreach (var item in m_RolesList)
            {
                if (item==role)
                {
                    m_AccountsRepository.UpdateRole(new AccountsDto { role = role, id = id });
                    return true;
                }
            }
            return false;
        }
        public bool UpdateLogin(string login, string newLogin)
        {
            List<string> lst = m_AccountsRepository.GetUserLogins();
            foreach (var item in lst)
            {
                if (newLogin == item)
                {
                    return false;
                }
            }
            int id = Convert.ToInt32(m_AccountsRepository.GetUserByLogin(new AccountsDto() { login = login })[0]);
            m_AccountsRepository.UpdateLogin(new AccountsDto() { id = id, login = newLogin}) ;
            return true;
        }
        public bool UpdateEmail(string login, string email)
        {
            List<string> lst = m_AccountsRepository.GetUserEmails();
            foreach (var item in lst)
            {
                if (email == item)
                {
                    return false;
                }
            }
            int id = Convert.ToInt32(m_AccountsRepository.GetUserByLogin(new AccountsDto() { login = login })[0]);
            m_AccountsRepository.UpdateEmail(new AccountsDto() { id = id, email = email });
            return true;
        }
        public void UpdatePassword(string login, string password)
        {
            int id = Convert.ToInt32(m_AccountsRepository.GetUserByLogin(new AccountsDto() { login = login })[0]);
            m_AccountsRepository.UpdatePassword(new AccountsDto() { id = id, password=EncryptPlainTextToCipherText(password) });
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
            string login = m_AccountsRepository.GetLoginByEmail(new AccountsDto { email = email })[0];
            string password = RandomString(8);
            UpdatePassword(login, password);          
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Восстановление пароля";
            m.Body = "<h2>Здравствуйте, недавно вы пытались восстановить данные своего аккаунта.</h2><span>Ваш логин: "+login+" Ваш пароль:"+password+"</span>";
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("cmspassreq@gmail.com", "1234ww1234ww");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
        private const string SecurityKey = "NeroClaudiusIsBest22899321";

        public static string EncryptPlainTextToCipherText(string PlainText)
        {
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();
            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            objTripleDESCryptoService.Key = securityKeyArray;
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;
            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
      
    }
       
    
}

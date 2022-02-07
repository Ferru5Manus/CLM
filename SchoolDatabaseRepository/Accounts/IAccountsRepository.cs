using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDatabaseRepository
{
    public interface IAccountsRepository
    {
        void AddAccount(AccountsDto account);
        List<string> GetAllUsersIds();
        List<string> GetUserLogins();
        List<string> GetUserEmails();
        List<string> GetUserRoles();
        List<string> GetUserForms();
        void UpdateLogin(AccountsDto account);
        void UpdateEmail(AccountsDto account);
        void UpdateForm(AccountsDto account);
        void UpdateRole(AccountsDto account);
        void GetPassword(AccountsDto accounts);
        List<string> GetUserByLogin(AccountsDto account);
    }
}

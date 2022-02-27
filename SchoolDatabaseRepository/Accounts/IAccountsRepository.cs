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

        void UpdateForm(AccountsDto account);
        void UpdateRole(AccountsDto account);
        List<string> GetPasswordByEmail(AccountsDto accounts);
        List<string> GetPasswordByLogin(AccountsDto accounts);
        List<string> GetLoginByEmail(AccountsDto accounts);
        List<string> GetUserByLogin(AccountsDto account);
        List<string> GetUserByEmail(AccountsDto account);
        List<string> GetRoleByLogin(AccountsDto accounts);
        List<string> GetLoginsByForm(AccountsDto accounts);
        List<string> GetFormByLogin(AccountsDto accounts);

    }
}

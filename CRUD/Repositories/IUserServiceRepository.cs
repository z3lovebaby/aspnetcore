using CRUD.Models;
using Microsoft.AspNetCore.Identity;

namespace CRUD.Repositories
{
    public interface IUserServiceRepository
    {
        Task<bool> IsValidUserAsync(SignIn users);

        UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens user);

        UserRefreshTokens GetSavedRefreshTokens(string username, string refreshtoken);

        void DeleteUserRefreshTokens(string username, string refreshToken);

        int SaveCommit();
    }
}

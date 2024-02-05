using CRUD.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRUD.Repositories
{
    public class UserServiceRepository : IUserServiceRepository
    {
        private readonly CRUDContext _db;
        private readonly SignInManager<AppUsers> signInManager;
        public UserServiceRepository(SignInManager<AppUsers> signInManager, CRUDContext db)
        {
            this.signInManager = signInManager;
            this._db = db;
        }

        public UserRefreshTokens AddUserRefreshTokens(UserRefreshTokens user)
        {
            _db.UserRefreshToken.Add(user);
            return user;
        }

        public void DeleteUserRefreshTokens(string username, string refreshToken)
        {
            var item = _db.UserRefreshToken.FirstOrDefault(x => x.UserName == username && x.RefreshToken == refreshToken);
            if (item != null)
            {
                _db.UserRefreshToken.Remove(item);
            }
        }

        public UserRefreshTokens GetSavedRefreshTokens(string username, string refreshToken)
        {
            return _db.UserRefreshToken.FirstOrDefault(x => x.UserName == username && x.RefreshToken == refreshToken && x.IsActive == true && DateTime.Now < x.ExpirationTime);
        }

        public int SaveCommit()
        {
            return _db.SaveChanges();
        }

        public async Task<bool> IsValidUserAsync(SignIn users)
        {
            var result = await signInManager.PasswordSignInAsync(
            users.Email, users.Password, false, false);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;

        }
    }
}

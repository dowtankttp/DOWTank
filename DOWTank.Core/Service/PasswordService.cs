using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOWTank.Core.Service
{
   public class PasswordService : IPasswordService
    {
        private readonly DOWTankEntities _context;

        public PasswordService(DOWTankEntities context)
        {
            _context = context;
        }

       public bool ValidateUserLogin(string userName, string password)
       {
           //TODO: retrive user details from db passed on username
           //var user = GetUserFromDb(username);
           //var salt = user.PasswordSalt;
           var salt = "";
           var hash = GeneratePasswordHash(password, salt);

           //return user.PasswordHash == hash;
           return true;
       }

       public string GeneratePasswordSalt()
       {
           return GetBufferAsBase64(64);
       }

        public string GeneratePasswordHash(string password, string salt)
        {
              byte[] hash = System.Security.Cryptography.SHA512.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(password + salt));

              return Convert.ToBase64String(hash);
        }

        private string GetBufferAsBase64(int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];

            //using (var rng = new RNGCryptoServiceProvider())
            //{
            //  rng.GetBytes(buffer);
            //  return Convert.ToBase64String(buffer);
            //}
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }
    }

   public interface IPasswordService
   {
       string GeneratePasswordSalt();
       string GeneratePasswordHash(string password, string salt);
       bool ValidateUserLogin(string userName, string password);
   }

}

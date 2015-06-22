using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DOWTank.Common;
using DOWTank.Core.Domain.TANK_usp_sel;

namespace DOWTank.Core.Service
{
   public class PasswordService : IPasswordService
    {
        private readonly DOWTankEntities _context;
        private readonly ISharedFunctions _sharedFunctions;

        public PasswordService(DOWTankEntities context,
                                ISharedFunctions sharedFunctions)
        {
            _context = context;
            _sharedFunctions = sharedFunctions;
        }

       public bool ValidateUserLogin(string userName, string password, ref TANK_usp_sel_Security_spResults userDetails)
       {
           userDetails = _sharedFunctions.GetUserDetails(userName);

           var salt = userDetails.PasswordSalt;
           var hash = GeneratePasswordHash(password, salt);

           return userDetails.PasswordHash == hash;
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
       bool ValidateUserLogin(string userName, string password, ref TANK_usp_sel_Security_spResults userDetails);
   }

}

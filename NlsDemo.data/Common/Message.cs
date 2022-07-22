using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NlsDemo.data.Common
{
    public static class Message
    {
        #region Common
        public static string Error = "Oops! Something want wrong please try again...";
        public static string Success = "Success";
        public static string NotFound = "Not found";
        public static string AlreadyExists = "User already exists!";
        public static string UnAuthorized = "User is unauthorized";
        public static string CredentialFailed = "User creation failed! Please check user details and try again.";
        #endregion
    }
}

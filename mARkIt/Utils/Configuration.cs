using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mARkIt.Utils
{
    public class Configuration
    {
        public const string FacebookAuthScope = "email";
        public const string FacebookRedirectUrl = "https://www.facebook.com/connect/login_success.html";
        public const string GoogleAuthScope = "email";
        public const string PackageName = "com.companyname.markit";
        public const string RedirectUrl = "/oauth2redirect";
        public const string GoogleRedirectUrl = "com.companyname.markit:/oauth2redirect";
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FrontEnd.Constants
{
    public class RestUrl
    {
        public RestUrl() { }
        public static string Base_url => "http://10.0.2.2:9090/api";

        public static string Login_url => "/login";
        public static string Registration_url => "/registration";

    }
}

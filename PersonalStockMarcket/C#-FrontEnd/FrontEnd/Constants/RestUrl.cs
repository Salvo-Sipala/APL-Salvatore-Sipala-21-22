using System;
using System.Collections.Generic;
using System.Text;

namespace FrontEnd.Constants
{
    public class RestUrl
    {
        public RestUrl() { }
        // this address is used for the Google emulators
        //public static string Base_url => "http://10.0.2.2:8081";

        // Ngrok site for create a tunnel between a local port (8081) of my local webserver
        // and a public URLs. So backend exposed in 8081 port to frontend with this site
        public static string Base_url => "http://1b32-79-25-77-182.ngrok.io"; // -> http://localhost:8081

        public static string Login_url => "/login";
        public static string Registration_url => "/register";

    }
}

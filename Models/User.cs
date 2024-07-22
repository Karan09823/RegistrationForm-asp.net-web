using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace REGISTRATION.Models
{
    public class User
    {

        public int registered_user_id { get; set; }

        public string registered_user_name { get; set; }

        public string registered_user_mail { get; set; }

        public String registered_user_password { get; set; }

        public string registered_user_about { get; set;}
    }
}
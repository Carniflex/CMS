using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class MyAdminmodel
    {
       
           
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string[] Roles { get; set; }
            public string NewRole { get; set; }

        
    }
}
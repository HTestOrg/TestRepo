using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UserAuthenticationModel
    {
        public Guid UserId { get; set; }

        public string DisplayName { get; set; }

        public string UserRole { get; set; }
    }
}

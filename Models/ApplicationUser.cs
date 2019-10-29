using System.Collections.Generic;

namespace ExcelExport.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public ICollection<Role> Roles {get;set;}
    }

    public class Role {
        public string RoleId { get; set; }
        public string Name { get; set; }
    }
}
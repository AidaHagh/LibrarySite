using Microsoft.AspNetCore.Identity;


namespace EntityLayer.Entities
{
    public class ApplicationRoles:IdentityRole
    {
   
        public string EnTitle { get; set; }
        public string FaTitle { get; set; }

    }
}

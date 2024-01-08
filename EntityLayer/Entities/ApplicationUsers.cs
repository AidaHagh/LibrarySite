using Microsoft.AspNetCore.Identity;


namespace EntityLayer.Entities
{
    public class ApplicationUsers :IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public byte Education { get; set; }
        public byte Gender { get; set; }
        public int Wallet { get; set; }

        //1 = Admin
        //2 = User
        public byte UserType { get; set; }
        public bool IsActive { get; set; }
    }
}

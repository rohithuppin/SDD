using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SDDUserApi.Data.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string EmailId { get; set; }
        public string PasswordHash { get; set; }
        //public string ConfirmPassword {  get; set; }
        //public Role Role { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDateTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Createdby { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}

namespace SDDUserApi.Data.Model
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Rolename { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Createdby { get; set; }
        public bool? IsActive { get; set; }
    }
}

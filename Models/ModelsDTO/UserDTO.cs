namespace ProjectWasel21.Models.ModelsDTO
{
    public class UserDTO
    {
        public int UserId { get; set; }   // (مهم للإدارة والربط)

        public string Username { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }
    }
}
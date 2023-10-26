namespace Dtos {
    public class UserSignUpDTO {
        public required string name { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
        }

     public class UserSignInDTO {
        public required string email { get; set; }
        public required string password { get; set; }
    }
}
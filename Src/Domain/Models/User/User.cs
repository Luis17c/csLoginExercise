using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models {
    [Table("users")]
    public class User : BaseEntity {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        [JsonIgnore]
        public string password { get; set; }

        public User(string name, string email, string password) {
            this.name = name;
            this.email = email;
            this.password = password;
        }
    }
}
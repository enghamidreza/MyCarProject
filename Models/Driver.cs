using System.ComponentModel.DataAnnotations;

namespace CarProject2.Models
{
    public class Driver
    {
        public int id { get; set; }
        [Required, StringLength(100)]
        public string FullName { get; set; }
        [Required, StringLength(10)]
        public string NationalCode { get; set; }
        //[Required, StringLength(15)]
        public string Phone { get; set; }

    }
}

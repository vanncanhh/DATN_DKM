using System.ComponentModel.DataAnnotations;

namespace QLTAISAN.Models
{
    public class RepairType
    {
        [Key]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Notes { get; set; }
    }
}

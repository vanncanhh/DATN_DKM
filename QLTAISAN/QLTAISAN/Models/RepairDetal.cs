namespace QLTAISAN.Models
{
    public class RepairDetal
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int RepairTypeId { get; set; }
        public Device Device { get; set; }
        public RepairType RepairType { get; set; }
    }
}

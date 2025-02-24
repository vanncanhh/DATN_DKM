using System;
using System.Collections.Generic;

namespace Libs.Models
{
    public partial class RequestDevice
    {
        public int Id { get; set; }
        public int? UserRequest { get; set; }
        public DateTime? DateOfRequest { get; set; }
        public DateTime? DateOfUse { get; set; }
        public string? DeviceName { get; set; }
        public int? TypeOfDevice { get; set; }
        public string? Configuration { get; set; }
        public string? Notes { get; set; }
        public bool? Approved { get; set; }
        public int? UserApproved { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Status { get; set; }
        public int? NumDevice { get; set; }
        public string? NoteProcess { get; set; }
        public string? NoteReasonRefuse { get; set; }
        public string? NameUserApproved { get; set; }

        public virtual DeviceType? TypeOfDeviceNavigation { get; set; }
        public virtual User? UserRequestNavigation { get; set; }
    }
}

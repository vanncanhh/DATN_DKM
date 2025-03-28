﻿namespace Libs.DTOs
{
    public class DeviceById_Result
    {
        public int Id { get; set; }
        public string? DeviceCode { get; set; }
        public string? DeviceName { get; set; }
        public Nullable<int> TypeOfDevice { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string? Configuration { get; set; }
        public Nullable<double> Price { get; set; }
        public string? PurchaseContract { get; set; }
        public Nullable<System.DateTime> DateOfPurchase { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public Nullable<System.DateTime> Guarantee { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string? ProjectName { get; set; }
        public Nullable<int> IdProject { get; set; }
        public Nullable<int> StatusRepair { get; set; }
        public string? Notes { get; set; }
        public string? NewCode { get; set; }
        public string? PriceOne { get; set; }
    }
}

﻿namespace QLTAISAN.Models
{
    public class SearchDevice_Result
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceCode { get; set; }
        public Nullable<double> Price { get; set; }
        public string TypeName { get; set; }
        public string FullName { get; set; }
        public string ProjectName { get; set; }
        public string Configuration { get; set; }
        public string Name { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> StatusRepair { get; set; }
        public string Notes { get; set; }
        public string NewCode { get; set; }
        public string ProjectSymbol { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string PriceOne { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> Status { get; set; }
        public string NameTypeComponant { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}

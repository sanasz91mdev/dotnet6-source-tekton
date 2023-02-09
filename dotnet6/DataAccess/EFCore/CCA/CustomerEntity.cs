using System;
using System.Collections.Generic;

namespace DataAccess.EFCore.CCA
{
    public partial class CustomerEntity
    {
        public string Customerid { get; set; } = null!;
        public string? Status { get; set; }
        public string? Cnic { get; set; }
        public string? Nic { get; set; }
        public string? Dateofbirth { get; set; }
        public string? Homephone { get; set; }
        public string? Mobilenumber { get; set; }
        public string? Officephone { get; set; }
        public string? Fathersname { get; set; }
        public string? Mothersname { get; set; }
        public string? Email { get; set; }
        public string? Company { get; set; }
        public string? Customertype { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Homeaddress1 { get; set; }
        public string? Homeaddress2 { get; set; }
        public string? Homeaddress3 { get; set; }
        public string? Homeaddress4 { get; set; }
        public string? Homeaddress5 { get; set; }
        public string? Officeaddress1 { get; set; }
        public string? Officeaddress2 { get; set; }
        public string? Officeaddress3 { get; set; }
        public string? Officeaddress4 { get; set; }
        public string? Officeaddress5 { get; set; }
        public string? Designation { get; set; }
        public string? Imd { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? Billingflag { get; set; }
        public string? Activiationdate { get; set; }
        public string? Lastupdateddate { get; set; }
        public string? Registrationdate { get; set; }
        public string? BranchCode { get; set; }
        public string? PassportNo { get; set; }
        public string? OldcustomerId { get; set; }
        public string? Transactionalerts { get; set; }
        public string? Channelalerts { get; set; }
        public string? Transactioninsurance { get; set; }
        public string? Faxnumber { get; set; }
        public string? Gender { get; set; }
        public string? InternalBranchId { get; set; }
        public string? Hostcustomerid { get; set; }
        public string? Homepostalcode { get; set; }
        public string? Officepostalcode { get; set; }
        public string? Maritalstatus { get; set; }
        public string? Province { get; set; }
        public string? Occupation { get; set; }
        public string? Nationality { get; set; }
        public string? Placeofbirth { get; set; }
        public string? Title { get; set; }
        public string? CustLanguage { get; set; }
        public string? IsFirstLogin { get; set; }
        public string? CustDispenseAlgo { get; set; }
        public string? EreceiptFlag { get; set; }
        public string? IsFundable { get; set; }
        public DateTime? Createdon { get; set; }
    }
}

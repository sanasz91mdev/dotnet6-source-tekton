using System;
using System.Collections.Generic;

namespace DataAccess.EFCore.CCA
{
    public partial class CardEntity
    {
        public string Cardnumber { get; set; } = null!;
        public string? Customerid { get; set; }
        public string? Issupplementarycard { get; set; }
        public string? Cardname { get; set; }
        public string? Cardstatus { get; set; }
        public string? Servicecode { get; set; }
        public string? Languagecode { get; set; }
        public string? Branchcode { get; set; }
        public string? Expirydate { get; set; }
        public string? Creationdate { get; set; }
        public string? Track1Data { get; set; }
        public string? Track2Data { get; set; }
        public string? Track3Data { get; set; }
        public string? Cvv1 { get; set; }
        public string? Cvv2 { get; set; }
        public string? Hubcode { get; set; }
        public string? Regioncode { get; set; }
        public string? Primarycardnumber { get; set; }
        public string? Activationdate { get; set; }
        public string? Processingcode { get; set; }
        public string? GroupId { get; set; }
        public string? Dataenterdate { get; set; }
        public string? IsIndividual { get; set; }
        public string? ModificationDate { get; set; }
        public string RelationshipId { get; set; } = null!;
        public string? Deliverystatus { get; set; }
        public string? Existingcardnumber { get; set; }
        public string? ExceptionfileStatus { get; set; }
        public string? RenewalFeePaid { get; set; }
        public string? ReplacementFeePaid { get; set; }
        public string? IssuanceFeePaid { get; set; }
        public string? RegenerationType { get; set; }
        public string? RenewalDate { get; set; }
        public string? RenewalStatus { get; set; }
        public string? Locationbranch { get; set; }
        public string? Jobid { get; set; }
        public decimal? Cardlocationstatus { get; set; }
        public string? Designcode { get; set; }
        public string? Issuancedate { get; set; }
        public string? Placementdatetime { get; set; }
        public string? Refundrequestdate { get; set; }
        public string? CardInterfacingNo { get; set; }
        public string? Firstcardnumber { get; set; }
        public string? XsAccount { get; set; }
        public string? Processrenewdate { get; set; }
        public string? Authcode { get; set; }
        public string? RecardStatus { get; set; }
        public string? RecardDate { get; set; }
        public string? RecardDestProduct { get; set; }
        public string? RecardNewAdc { get; set; }
        public string? RecardNewExtNet { get; set; }
        public string? RecardProcessDate { get; set; }
        public string? Authcodelastchange { get; set; }
        public string? Firstlogin { get; set; }
        public string? DataSource { get; set; }
        public string? BarcodePin { get; set; }
        public string? ReproduceStatus { get; set; }
        public string? TrackingId { get; set; }
        public decimal? Iscustomerlinked { get; set; }
        public string? Customerlinkedon { get; set; }
        public string? Customerlinkedby { get; set; }
        public string? ValidityStartDate { get; set; }
        public string? ValidityEndDate { get; set; }
        public string? Cardtrackingid { get; set; }
        public string? Corporateid { get; set; }
        public string? CardReferenceNo { get; set; }
    }
}

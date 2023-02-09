using System;
using System.Collections.Generic;

namespace DataAccess.EFCore.CCA
{
    public partial class CustChannelAccountEntity
    {
        public string? Accountmap { get; set; }
        public string ChannelId { get; set; } = null!;
        public string RelationshipId { get; set; } = null!;
        public string? IsDefault { get; set; }
        public string AccountId { get; set; } = null!;
        public string AccountType { get; set; } = null!;
        public string AccountCurrency { get; set; } = null!;
        public string? CustomerId { get; set; }
        public string? TranRestrict { get; set; }
        public string? Nature { get; set; }
        public string? RelationshipauthId { get; set; }
        public string Productid { get; set; } = null!;
        public string? Isblacklisted { get; set; }
        public string? Walletorder { get; set; }
        public decimal? PkCustchannelacc { get; set; }
        public decimal? CheckerRequestId { get; set; }
    }
}

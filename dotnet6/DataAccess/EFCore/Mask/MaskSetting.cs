using System;
using System.Collections.Generic;

namespace DataAccess.EFCore.Mask
{
    public partial class MaskSetting
    {
        public decimal SettingId { get; set; }
        public string? SettingName { get; set; }
        public string? SettingDescription { get; set; }
        public string? Active { get; set; }
        public decimal? IsDefault { get; set; }
        public string? PendingOperation { get; set; }
        public string? CheckerRequestId { get; set; }
        public decimal? CheckerPending { get; set; }
    }
}

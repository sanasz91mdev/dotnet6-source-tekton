using System;
using System.Collections.Generic;

namespace DataAccess.EFCore.Mask
{
    public partial class MaskSettingDetail
    {
        public decimal SettingId { get; set; }
        public decimal FieldId { get; set; }
        public decimal StartIndex { get; set; }
        public decimal Length { get; set; }
        public string? MaskCharacter { get; set; }
        public string? MaskType { get; set; }
        public string? PendingOperation { get; set; }
        public string? CheckerRequestId { get; set; }
        public decimal? CheckerPending { get; set; }
    }
}

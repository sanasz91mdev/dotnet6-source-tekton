using System;
using System.Collections.Generic;

namespace DataAccess.EFCore.Mask
{
    public partial class MaskField
    {
        public decimal FieldId { get; set; }
        public string? FieldName { get; set; }
        public string? Length { get; set; }
        public string? Tags { get; set; }
    }
}

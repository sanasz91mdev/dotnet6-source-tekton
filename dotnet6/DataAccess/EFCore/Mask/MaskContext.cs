using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Oracle.EntityFrameworkCore.Infrastructure;

namespace DataAccess.EFCore.Mask
{
    public partial class MaskContext : DbContext
    {
        public MaskContext()
        {
        }

        public MaskContext(DbContextOptions<MaskContext> options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseOracle("Data Source=192.168.7.228:1521/QM13;PERSIST SECURITY INFO=True;USER ID=iris; Password=tpstps;Pooling=false");
        //}

        public virtual DbSet<MaskField> MaskFields { get; set; } = null!;
        public virtual DbSet<MaskSetting> MaskSettings { get; set; } = null!;
        public virtual DbSet<MaskSettingDetail> MaskSettingDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //OracleDbContextOptionsBuilder builder = new OracleDbContextOptionsBuilder(optionsBuilder.UseOracle(""));
                //builder.UseOracleSQLCompatibility("11");
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("User Id=iris;Password=tpstps;Data Source=192.168.7.228:1521/QM13;Pooling=true;Max Pool Size = 200;Min Pool Size = 10;Connection Timeout=30");
                ;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("IRIS")
                .UseCollation("USING_NLS_COMP");

            modelBuilder.Entity<MaskField>(entity =>
            {
                entity.HasKey(e => e.FieldId)
                    .HasName("SYS_C0049172");

                entity.ToTable("MASK_FIELD", "IRIS_CONFIG");

                entity.Property(e => e.FieldId)
                    .HasColumnType("NUMBER(22)")
                    .HasColumnName("FIELD_ID");

                entity.Property(e => e.FieldName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIELD_NAME");

                entity.Property(e => e.Length)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LENGTH");

                entity.Property(e => e.Tags)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("TAGS");
            });

            modelBuilder.Entity<MaskSetting>(entity =>
            {
                entity.HasKey(e => e.SettingId)
                    .HasName("SYS_C0049179");

                entity.ToTable("MASK_SETTING", "IRIS_CONFIG");

                entity.Property(e => e.SettingId)
                    .HasColumnType("NUMBER(22)")
                    .HasColumnName("SETTING_ID");

                entity.Property(e => e.Active)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVE");

                entity.Property(e => e.CheckerPending)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CHECKER_PENDING");

                entity.Property(e => e.CheckerRequestId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CHECKER_REQUEST_ID");

                entity.Property(e => e.IsDefault)
                    .HasColumnType("NUMBER(22)")
                    .HasColumnName("IS_DEFAULT");

                entity.Property(e => e.PendingOperation)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PENDING_OPERATION");

                entity.Property(e => e.SettingDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SETTING_DESCRIPTION");

                entity.Property(e => e.SettingName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SETTING_NAME");
            });

            modelBuilder.Entity<MaskSettingDetail>(entity =>
            {
                entity.HasKey(e => new { e.SettingId, e.FieldId })
                    .HasName("MASK_SETTING_DETAIL_IDX1_PK");

                entity.ToTable("MASK_SETTING_DETAIL", "IRIS_CONFIG");

                entity.Property(e => e.SettingId)
                    .HasColumnType("NUMBER(22)")
                    .HasColumnName("SETTING_ID");

                entity.Property(e => e.FieldId)
                    .HasColumnType("NUMBER(22)")
                    .HasColumnName("FIELD_ID");

                entity.Property(e => e.CheckerPending)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CHECKER_PENDING");

                entity.Property(e => e.CheckerRequestId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CHECKER_REQUEST_ID");

                entity.Property(e => e.Length)
                    .HasColumnType("NUMBER(22)")
                    .HasColumnName("LENGTH");

                entity.Property(e => e.MaskCharacter)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MASK_CHARACTER");

                entity.Property(e => e.MaskType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MASK_TYPE");

                entity.Property(e => e.PendingOperation)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PENDING_OPERATION");

                entity.Property(e => e.StartIndex)
                    .HasColumnType("NUMBER(22)")
                    .HasColumnName("START_INDEX");
            });

            modelBuilder.HasSequence("AUTO_INCREMENT", "IRIS_CONFIG");

            modelBuilder.HasSequence("PAYMENT_COMP_IMPCONFIG_ID_GEN", "IRIS_CONFIG");

            modelBuilder.HasSequence("RELATIONSHIP_AUTH_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_ALERT_TRIGGER_APPLICANT_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_APP_FEEDBACK", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_ATM_COMPONENT_TRIGGER", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_BIC_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_BIN_PARAMETER_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_BIN_PARAMETER_PROFILE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_CHANNEL_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_COMMISSION_DISCOUNT_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_CUST_DISCOUNT_PROFILE", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_CUSTID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_CUSTOMERID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_DISCOUNT_PROFILE_SLAB", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_EMV_PARAMETER_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_ENTITY_TAG_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_ESCROW_FEE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_ESCROW_PROFILE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_FILE_LOCATION_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_FORMATHISTORY_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_HIERARCHY_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_HIERARCHY_LEVEL_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_HIERARCHY_LINKING_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_HIERARCHY_ROLE", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_HIERARCHYRELATIONRULE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_HSM_CONNECTION_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_HSM_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_IMP_FORMAT_SCH", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_INST_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_KEY_GENERATION", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_KEY_PROFILE_CODE", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_LIMIT_CONFIGURATION_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_LIMIT_COUNT_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_LIMIT_COUNT_TYPE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_LIMIT_RULE_CRITERIA_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_LIMIT_RULE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_LIMITAMOUNT", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MANUAL_TRANS_DEFINITION", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MANUAL_TRANS_POSTING", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERC_CHANNEL_LINKING", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_ACC_LINKING_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_ACCOUNT_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_BANK_LINKING_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_CATEGORY_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_CODE", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_CONTACT_PERSON_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_GROUP_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_HIERARCHY_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_LEVEL_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_LIMIT_PROFILE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_LISTING_REPORT", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_PAN_FORMAT_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_PARAMETER_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_PAYMENT_HISTORY", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_REF_NO", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_STATEMENT_REPORT", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_TEMPLATE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MERCHANT_TRAN_DETAIL_REP", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MSG_FORMAT", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MSG_MAPPING", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_MSG_TYPE", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_NETWORK_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_NOTIFICATION_EVENTS", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PAN_FORMAT_DETAIL_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PAYMENT_COMPANY_ACCOUNT", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PAYMENT_COMPANY_IMPFORMAT", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PAYMENT_ENTITY_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PAYMENTCOMPANY_CODE", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PE_TRX_CHNL_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PORTAL_ENTITY_ROLE_LINKING", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PORTAL_ENTITY_USER_LINKING", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PORTAL_PERMISSION_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PORTAL_ROLEID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PORTAL_USERID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_POS_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_POS_PARAMETER_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_POS_TEMPLATE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_POS_TRANSACTIONS", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_POS_VERSION", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PREFIX_PARAM_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PREFIX_PARAM_PROFILE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PREFIX_SUBPARAM_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PROCESS_GROUP_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PROD_MIG_HISTORY_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PRODUCT_MIGRATION_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_PUSH_PAYMENT_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_RECORD_DATA", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_REPORT_PARAM_FILL_OBJECT", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_REPORT_PARAMETERS", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_REPORT_SP", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_REPORT_SP_PARAMETER", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_REPORTS", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_ROUTING_RULE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_RULE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_SECRETANSWER", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_SECRETQUESION", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_SETTLEMENT_RULE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_SETTLERULECOND_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_SPLIT_COMMISSION_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_TAG_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_TBLBRANCH_INTERN_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_TRANS_PRICING_METHOD_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_TRANS_SUMMARY_REPORT", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_TRANSACTION_ALERT_TYPE_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_TRANSACTION_PRICING_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_USER_ALERT_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_USER_AUTH_CODE", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_USER_EVENT_ID", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_USER_EVENT_NOTIFICATIONS", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQ_USER_STATUS_HISTORY", "IRIS_CONFIG");

            modelBuilder.HasSequence("SEQCARDPRODUCTION", "IRIS_CONFIG");

            modelBuilder.HasSequence("TBLCFGPRODUCTDETAI_SEQ", "IRIS_CONFIG");

            modelBuilder.HasSequence("USER_DEVICE_ID", "IRIS_CONFIG");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

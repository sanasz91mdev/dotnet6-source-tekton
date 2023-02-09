using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace DataAccess.EFCore.CCA
{
    public partial class CCAContext : DbContext
    {
        public CCAContext()
        {
        }

        public CCAContext(DbContextOptions<CCAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CustChannelAccountEntity> Tblcustchannelaccts { get; set; } = null!;
        public virtual DbSet<CustomerEntity> Tblcustomers { get; set; } = null!;
        public virtual DbSet<CardEntity> Tbldebitcards { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("IRIS");

            modelBuilder.Entity<CustChannelAccountEntity>(entity =>
            {
                entity.HasKey(e => new { e.ChannelId, e.RelationshipId, e.Productid, e.AccountId, e.AccountType, e.AccountCurrency })
                    .HasName("TBLCUSTCHANNELACCT_PK");

                entity.ToTable("TBLCUSTCHANNELACCT", "IRIS_CMS");

                entity.HasIndex(e => new { e.ChannelId, e.Productid, e.RelationshipId }, "TBLCUSTCHANNELACCT_INDEX1");

                entity.Property(e => e.ChannelId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CHANNEL_ID");

                entity.Property(e => e.RelationshipId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("RELATIONSHIP_ID");

                entity.Property(e => e.Productid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("PRODUCTID");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(28)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_ID");

                entity.Property(e => e.AccountType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_TYPE");

                entity.Property(e => e.AccountCurrency)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_CURRENCY");

                entity.Property(e => e.Accountmap)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNTMAP");

                entity.Property(e => e.CheckerRequestId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CHECKER_REQUEST_ID");

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_ID");

                entity.Property(e => e.IsDefault)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("IS_DEFAULT");

                entity.Property(e => e.Isblacklisted)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ISBLACKLISTED");

                entity.Property(e => e.Nature)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NATURE");

                entity.Property(e => e.PkCustchannelacc)
                    .HasColumnType("NUMBER")
                    .HasColumnName("PK_CUSTCHANNELACC");

                entity.Property(e => e.RelationshipauthId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RELATIONSHIPAUTH_ID");

                entity.Property(e => e.TranRestrict)
                    .IsUnicode(false)
                    .HasColumnName("TRAN_RESTRICT");

                entity.Property(e => e.Walletorder)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("WALLETORDER");
            });

            modelBuilder.Entity<CustomerEntity>(entity =>
            {
                entity.HasKey(e => e.Customerid)
                    .HasName("TBLCUSTOMER_PK11129286068312");

                entity.ToTable("TBLCUSTOMER", "IRIS_CMS");

                entity.HasIndex(e => e.BranchCode, "BRANCH_INDEX");

                entity.HasIndex(e => e.City, "IND_CITY_CUST");

                entity.HasIndex(e => e.Firstname, "TBLCUSTOMER_IND_CAPNAME");

                entity.HasIndex(e => e.Cnic, "TBLCUSTOMER_IND_CNIC");

                entity.HasIndex(e => new { e.BranchCode, e.Cnic, e.Nic }, "TBLCUSTOMER_IND_COM1");

                entity.HasIndex(e => e.Status, "TBLCUSTOMER_IND_STATUS");

                entity.HasIndex(e => e.Customertype, "TBLCUSTOMER_IND_TYPE");

                entity.HasIndex(e => e.Customerid, "TBLCUSTOMER_PK11129286068312")
                    .IsUnique();

                entity.Property(e => e.Customerid)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CUSTOMERID");

                entity.Property(e => e.Activiationdate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVIATIONDATE");

                entity.Property(e => e.Billingflag)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BILLINGFLAG");

                entity.Property(e => e.BranchCode)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_CODE");

                entity.Property(e => e.Channelalerts)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CHANNELALERTS");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CITY");

                entity.Property(e => e.Cnic)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("CNIC");

                entity.Property(e => e.Company)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COMPANY");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COUNTRY");

                entity.Property(e => e.Createdon)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATEDON")
                    .HasDefaultValueSql("SYSDATE\n   ");

                entity.Property(e => e.CustDispenseAlgo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CUST_DISPENSE_ALGO");

                entity.Property(e => e.CustLanguage)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("CUST_LANGUAGE");

                entity.Property(e => e.Customertype)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMERTYPE");

                entity.Property(e => e.Dateofbirth)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("DATEOFBIRTH");

                entity.Property(e => e.Designation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESIGNATION");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.EreceiptFlag)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ERECEIPT_FLAG");

                entity.Property(e => e.Fathersname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FATHERSNAME");

                entity.Property(e => e.Faxnumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FAXNUMBER");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTNAME");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("GENDER");

                entity.Property(e => e.Homeaddress1)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("HOMEADDRESS1");

                entity.Property(e => e.Homeaddress2)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("HOMEADDRESS2");

                entity.Property(e => e.Homeaddress3)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("HOMEADDRESS3");

                entity.Property(e => e.Homeaddress4)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("HOMEADDRESS4");

                entity.Property(e => e.Homeaddress5)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("HOMEADDRESS5");

                entity.Property(e => e.Homephone)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPHONE");

                entity.Property(e => e.Homepostalcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("HOMEPOSTALCODE");

                entity.Property(e => e.Hostcustomerid)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("HOSTCUSTOMERID");

                entity.Property(e => e.Imd)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("IMD");

                entity.Property(e => e.InternalBranchId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("INTERNAL_BRANCH_ID");

                entity.Property(e => e.IsFirstLogin)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_FIRST_LOGIN");

                entity.Property(e => e.IsFundable)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("IS_FUNDABLE")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LASTNAME");

                entity.Property(e => e.Lastupdateddate)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("LASTUPDATEDDATE");

                entity.Property(e => e.Maritalstatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MARITALSTATUS");

                entity.Property(e => e.Middlename)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("MIDDLENAME");

                entity.Property(e => e.Mobilenumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOBILENUMBER");

                entity.Property(e => e.Mothersname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOTHERSNAME");

                entity.Property(e => e.Nationality)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("NATIONALITY");

                entity.Property(e => e.Nic)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NIC");

                entity.Property(e => e.Occupation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("OCCUPATION");

                entity.Property(e => e.Officeaddress1)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("OFFICEADDRESS1");

                entity.Property(e => e.Officeaddress2)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("OFFICEADDRESS2");

                entity.Property(e => e.Officeaddress3)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("OFFICEADDRESS3");

                entity.Property(e => e.Officeaddress4)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("OFFICEADDRESS4");

                entity.Property(e => e.Officeaddress5)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("OFFICEADDRESS5");

                entity.Property(e => e.Officephone)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("OFFICEPHONE");

                entity.Property(e => e.Officepostalcode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("OFFICEPOSTALCODE");

                entity.Property(e => e.OldcustomerId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("OLDCUSTOMER_ID");

                entity.Property(e => e.PassportNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("PASSPORT_NO");

                entity.Property(e => e.Placeofbirth)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PLACEOFBIRTH");

                entity.Property(e => e.Province)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PROVINCE");

                entity.Property(e => e.Registrationdate)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("REGISTRATIONDATE");

                entity.Property(e => e.Status)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.Title)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("TITLE");

                entity.Property(e => e.Transactionalerts)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TRANSACTIONALERTS");

                entity.Property(e => e.Transactioninsurance)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TRANSACTIONINSURANCE");
            });

            modelBuilder.Entity<CardEntity>(entity =>
            {
                entity.HasKey(e => new { e.Cardnumber, e.RelationshipId })
                    .HasName("TBLDEBITCARD_PK");

                entity.ToTable("TBLDEBITCARD", "IRIS_CMS");

                entity.Property(e => e.Cardnumber)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CARDNUMBER");

                entity.Property(e => e.RelationshipId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("RELATIONSHIP_ID");

                entity.Property(e => e.Activationdate)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVATIONDATE");

                entity.Property(e => e.Authcode)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("AUTHCODE");

                entity.Property(e => e.Authcodelastchange)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("AUTHCODELASTCHANGE");

                entity.Property(e => e.BarcodePin)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("BARCODE_PIN");

                entity.Property(e => e.Branchcode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("BRANCHCODE");

                entity.Property(e => e.CardInterfacingNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CARD_INTERFACING_NO");

                entity.Property(e => e.CardReferenceNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CARD_REFERENCE_NO");

                entity.Property(e => e.Cardlocationstatus)
                    .HasColumnType("NUMBER")
                    .HasColumnName("CARDLOCATIONSTATUS");

                entity.Property(e => e.Cardname)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CARDNAME");

                entity.Property(e => e.Cardstatus)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("CARDSTATUS");

                entity.Property(e => e.Cardtrackingid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("CARDTRACKINGID");

                entity.Property(e => e.Corporateid)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("CORPORATEID");

                entity.Property(e => e.Creationdate)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("CREATIONDATE");

                entity.Property(e => e.Customerid)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMERID");

                entity.Property(e => e.Customerlinkedby)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMERLINKEDBY");

                entity.Property(e => e.Customerlinkedon)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMERLINKEDON");

                entity.Property(e => e.Cvv1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CVV1");

                entity.Property(e => e.Cvv2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CVV2");

                entity.Property(e => e.DataSource)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DATA_SOURCE");

                entity.Property(e => e.Dataenterdate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("DATAENTERDATE");

                entity.Property(e => e.Deliverystatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DELIVERYSTATUS");

                entity.Property(e => e.Designcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESIGNCODE");

                entity.Property(e => e.ExceptionfileStatus)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("EXCEPTIONFILE_STATUS");

                entity.Property(e => e.Existingcardnumber)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("EXISTINGCARDNUMBER");

                entity.Property(e => e.Expirydate)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("EXPIRYDATE");

                entity.Property(e => e.Firstcardnumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTCARDNUMBER");

                entity.Property(e => e.Firstlogin)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FIRSTLOGIN");

                entity.Property(e => e.GroupId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("GROUP_ID");

                entity.Property(e => e.Hubcode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("HUBCODE");

                entity.Property(e => e.IsIndividual)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("IS_INDIVIDUAL");

                entity.Property(e => e.Iscustomerlinked)
                    .HasColumnType("NUMBER(22)")
                    .HasColumnName("ISCUSTOMERLINKED")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.IssuanceFeePaid)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ISSUANCE_FEE_PAID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Issuancedate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ISSUANCEDATE");

                entity.Property(e => e.Issupplementarycard)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ISSUPPLEMENTARYCARD");

                entity.Property(e => e.Jobid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("JOBID");

                entity.Property(e => e.Languagecode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("LANGUAGECODE");

                entity.Property(e => e.Locationbranch)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LOCATIONBRANCH");

                entity.Property(e => e.ModificationDate)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MODIFICATION_DATE");

                entity.Property(e => e.Placementdatetime)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PLACEMENTDATETIME");

                entity.Property(e => e.Primarycardnumber)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("PRIMARYCARDNUMBER");

                entity.Property(e => e.Processingcode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("PROCESSINGCODE");

                entity.Property(e => e.Processrenewdate)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PROCESSRENEWDATE");

                entity.Property(e => e.RecardDate)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RECARD_DATE");

                entity.Property(e => e.RecardDestProduct)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RECARD_DEST_PRODUCT");

                entity.Property(e => e.RecardNewAdc)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RECARD_NEW_ADC");

                entity.Property(e => e.RecardNewExtNet)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RECARD_NEW_EXT_NET");

                entity.Property(e => e.RecardProcessDate)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RECARD_PROCESS_DATE");

                entity.Property(e => e.RecardStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RECARD_STATUS");

                entity.Property(e => e.Refundrequestdate)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REFUNDREQUESTDATE");

                entity.Property(e => e.RegenerationType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REGENERATION_TYPE")
                    .HasDefaultValueSql("3");

                entity.Property(e => e.Regioncode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("REGIONCODE");

                entity.Property(e => e.RenewalDate)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RENEWAL_DATE");

                entity.Property(e => e.RenewalFeePaid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RENEWAL_FEE_PAID");

                entity.Property(e => e.RenewalStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RENEWAL_STATUS");

                entity.Property(e => e.ReplacementFeePaid)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REPLACEMENT_FEE_PAID");

                entity.Property(e => e.ReproduceStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("REPRODUCE_STATUS");

                entity.Property(e => e.Servicecode)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("SERVICECODE");

                entity.Property(e => e.Track1Data)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TRACK1_DATA");

                entity.Property(e => e.Track2Data)
                    .HasMaxLength(107)
                    .IsUnicode(false)
                    .HasColumnName("TRACK2_DATA");

                entity.Property(e => e.Track3Data)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TRACK3_DATA");

                entity.Property(e => e.TrackingId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TRACKING_ID");

                entity.Property(e => e.ValidityEndDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VALIDITY_END_DATE");

                entity.Property(e => e.ValidityStartDate)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VALIDITY_START_DATE");

                entity.Property(e => e.XsAccount)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("XS_ACCOUNT");
            });

            modelBuilder.HasSequence("AUTO_INCREMENT", "IRIS_CMS");

            modelBuilder.HasSequence("FEEWAIVERSEQ", "IRIS_CMS");

            modelBuilder.HasSequence("GROUPIDSEQ", "IRIS_CMS");

            modelBuilder.HasSequence("IMD_PANRANGE", "IRIS_CMS").IsCyclic();

            modelBuilder.HasSequence("KYC_FIELD_EVENT_OPERATIONS_SEQ", "IRIS_CMS");

            modelBuilder.HasSequence("KYC_FIELD_OPEATION_INPUTVALUE", "IRIS_CMS");

            modelBuilder.HasSequence("KYC_FIELD_OPERATION_INPUTS_SEQ", "IRIS_CMS");

            modelBuilder.HasSequence("PRODUCTIONCYCLE", "IRIS_CMS");

            modelBuilder.HasSequence("RECEIPT_SEQ_ID", "IRIS_CMS");

            modelBuilder.HasSequence("RELATIONSHIP_AUTH_ID", "IRIS_CMS");

            modelBuilder.HasSequence("RELCHARGEID", "IRIS_CMS");

            modelBuilder.HasSequence("RULEIDSEQ", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_ACCOUNT_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_ACCOUNT_RANGE", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_ACKDETAILID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_ACKID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_ACTIVITYLOG", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_ALERT_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_BAL_KYC_CHECK", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_BAL_PROF", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_BAL_PROF_LIMIT", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_BULK_INVEN_LOG", "IRIS_CMS").IsCyclic();

            modelBuilder.HasSequence("SEQ_CARDFEE", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CARDFORMAT", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CARDFORMATDETAIL", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CARDPRODUCTIONEXPORTREQ", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CARDTRACKINGID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CHARGELOGDETAIL", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CORPORATE_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CRMTRANSACTIONLOG", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CUST_CORPORATE_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CUST_STATEMENT_REQUESTID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_CUSTOMERID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_DOC_HISTORYID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_DOC_STATUSHISTORYID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_FILEUPLOAD", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_FORCESTAGINGID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_HOSTLIMIT_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_INTERNATIONALTRAN", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_INV_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_INV_LOG_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_INV_PLACE_LOG", "IRIS_CMS").IsCyclic();

            modelBuilder.HasSequence("SEQ_JOB_NO", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_KYCPROFILE", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_LEAN_ACCOUNTID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_LEVEL_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_LIMITAMOUNT", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_LIMITPROFILE", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_LOGID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_MACHINE", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_MANUALTRAN", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PHYINLOGID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PINMAILER_SERIAL", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PINMAILERFORMAT", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PINMAILERFORMATDETAIL", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PROD_SERVICE_ALERT", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PROD_TXN_ALERT_TRIGGER", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PRODMIGRATION_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PRODUCT", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PRODUCT_DETAIL", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PRODUCTCODE", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PRODUCTPAYMENTINSTRUMENT", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_PROF_DET", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_RELCHARGE", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_RRN", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_SERVICE_EVENT_ID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_STAN", "IRIS_CMS").IsCyclic();

            modelBuilder.HasSequence("SEQ_TBLCFGPRODUCTACCOUNT", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_TBLCFGPRODUCTWALLET", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_TBLCFGPRODUCTWALLETSVA", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_TBLPRODUCTIONLOG", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_TRACKFORMAT1", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_TRACKFORMAT2", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_TRAILID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_VBVSETTINGID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQ_VERIFICATIIONSCHEME", "IRIS_CMS");

            modelBuilder.HasSequence("SEQAMLID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQCARDPRODUCTION", "IRIS_CMS");

            modelBuilder.HasSequence("SEQFEEWAIVERID", "IRIS_CMS");

            modelBuilder.HasSequence("SEQNOTE", "IRIS_CMS");

            modelBuilder.HasSequence("SEQSEGMENTID", "IRIS_CMS");

            modelBuilder.HasSequence("TBLCFGFEEHANDLER_SEQ", "IRIS_CMS");

            modelBuilder.HasSequence("TBLCFGFEEHANDLERDETAIL_SEQ", "IRIS_CMS");

            modelBuilder.HasSequence("TBLCHARGELOG_SEQ", "IRIS_CMS");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

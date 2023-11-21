﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XRFID.Demo.Server.Database;

#nullable disable

namespace XRFID.Demo.Server.Migrations
{
    [DbContext(typeof(XRFIDSampleContext))]
    partial class XRFIDSampleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Label", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Language")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifierUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.LoadingUnit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsConsolidated")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsValid")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifierUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderReference")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ReaderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Sequence")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ReaderId");

                    b.ToTable("LoadingUnits");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.LoadingUnitItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Epc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsConsolidated")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifierUserId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("LoadingUnitId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoadingUnitReference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LoadingUnitId");

                    b.ToTable("LoadingUnitItems");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Movement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Direction")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsConsolidated")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsValid")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifierUserId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("MissingItem")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderReference")
                        .HasColumnType("TEXT");

                    b.Property<bool>("OverflowItem")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ReaderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Sequence")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("UnexpectedItem")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ReaderId");

                    b.ToTable("Movements");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.MovementItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Checked")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Epc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FirstRead")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("IgnoreUntil")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsConsolidated")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsValid")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifierUserId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastRead")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("LoadingUnitItemId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MovementId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PC")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReadsCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<short>("Rssi")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tid")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LoadingUnitItemId");

                    b.HasIndex("MovementId");

                    b.HasIndex("ProductId");

                    b.ToTable("MovementItems");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Printer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Language")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifierUserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("LicenseStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MacAddress")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Manufacturer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Version")
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkflowType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Printers");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ContentQuantity")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Epc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifierUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SkuId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Epc")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("SkuId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Reader", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("ActiveMovementId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CorrelationId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("GpoConfiguration")
                        .HasColumnType("TEXT");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifierUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("MacAddress")
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReaderOS")
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Uid")
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Readers");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Sku", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EffectivityEnd")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EffectivityStart")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastModifierUserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Skus");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.LoadingUnit", b =>
                {
                    b.HasOne("XRFID.Demo.Server.Entities.Reader", "Reader")
                        .WithMany("LoadingUnits")
                        .HasForeignKey("ReaderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Reader");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.LoadingUnitItem", b =>
                {
                    b.HasOne("XRFID.Demo.Server.Entities.LoadingUnit", "LoadingUnit")
                        .WithMany("LoadingUnitItems")
                        .HasForeignKey("LoadingUnitId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("LoadingUnit");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Movement", b =>
                {
                    b.HasOne("XRFID.Demo.Server.Entities.Reader", "Reader")
                        .WithMany("Movements")
                        .HasForeignKey("ReaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reader");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.MovementItem", b =>
                {
                    b.HasOne("XRFID.Demo.Server.Entities.LoadingUnitItem", "LoadingUnitItem")
                        .WithMany()
                        .HasForeignKey("LoadingUnitItemId");

                    b.HasOne("XRFID.Demo.Server.Entities.Movement", "Movement")
                        .WithMany("MovementItems")
                        .HasForeignKey("MovementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("XRFID.Demo.Server.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.Navigation("LoadingUnitItem");

                    b.Navigation("Movement");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Product", b =>
                {
                    b.HasOne("XRFID.Demo.Server.Entities.Sku", "Sku")
                        .WithMany("Products")
                        .HasForeignKey("SkuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sku");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.LoadingUnit", b =>
                {
                    b.Navigation("LoadingUnitItems");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Movement", b =>
                {
                    b.Navigation("MovementItems");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Reader", b =>
                {
                    b.Navigation("LoadingUnits");

                    b.Navigation("Movements");
                });

            modelBuilder.Entity("XRFID.Demo.Server.Entities.Sku", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}

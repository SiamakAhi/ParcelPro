using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParcelPro.Areas.Courier.Models.Entities;

namespace ParcelPro.Areas.Courier.Models.Mapping
{
    public class Cu_BillOfLading_Config : IEntityTypeConfiguration<Cu_BillOfLading>
    {
        public void Configure(EntityTypeBuilder<Cu_BillOfLading> builder)
        {
            builder.HasKey(n => n.Id);

            builder.HasOne(n => n.IssuingBranch)
                .WithMany(n => n.OriginBills)
                .HasForeignKey(n => n.OriginBranchId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.OriginHub)
              .WithMany(n => n.BillOfLadings)
              .HasForeignKey(n => n.OriginHubId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Sender)
            .WithMany(n => n.Senders)
            .HasForeignKey(n => n.SenderId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Receiver)
           .WithMany(n => n.Recivers)
           .HasForeignKey(n => n.ReceiverId)
           .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Route)
          .WithMany(n => n.BillOfLadings)
          .HasForeignKey(n => n.RouteId)
          .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Service)
           .WithMany(n => n.BillOfLadings)
           .HasForeignKey(n => n.ServiceId)
           .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.CuInvoice)
           .WithMany(n => n.Items)
           .HasForeignKey(n => n.InvoiceId)
           .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.DistributerBranch)
             .WithMany(n => n.Distributers)
             .HasForeignKey(n => n.DistributerBranchId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.Contract)
            .WithMany(n => n.BillOfLadings)
            .HasForeignKey(n => n.ContractId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.BusinessPartner)
         .WithMany(n => n.BillOfLadings)
         .HasForeignKey(n => n.BusinessPartnerId)
         .OnDelete(DeleteBehavior.NoAction);

        }
    }
}

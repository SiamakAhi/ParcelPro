using ParcelPro.Areas.Organization.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ParcelPro.Areas.Organization.Models.modelConfig
{
    public class OrgDepartmentMapp : IEntityTypeConfiguration<OrgDepartment>
    {
        public void Configure(EntityTypeBuilder<OrgDepartment> builder)
        {
            builder.HasKey(n => n.Id); // کلید اصلی

            builder.Property(n => n.Name)
                .IsRequired();


            builder.Property(n => n.Code)
                .IsRequired()
                .HasMaxLength(50); // تنظیم حداکثر طول کد

            builder.HasOne(n => n.ParentDepartment)
                .WithMany(n => n.ChildDepartments)
                .HasForeignKey(n => n.ParentDepartmentId)
                .OnDelete(DeleteBehavior.Restrict); // ارتباط بخش والد و فرزندان
        }
    }
}

using ParcelPro.Areas.Accounting.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ParcelPro.Areas.Accounting.Models.Mapping
{
    public class Acc_Coding_Tafsil_Mapp : IEntityTypeConfiguration<Acc_Coding_Tafsil>
    {
        public void Configure(EntityTypeBuilder<Acc_Coding_Tafsil> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}

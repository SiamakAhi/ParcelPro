using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Areas.AvaRasta.Models.Entities
{
    public class Module
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<License> Licenses { get; set; }
    }

}

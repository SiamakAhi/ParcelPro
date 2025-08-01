using ParcelPro.Areas.Warehouse.Dto;

namespace ParcelPro.Areas.Warehouse.Models.Dtos
{
    public class VmUnitOfMeasures
    {
        public List<UnitOfMeasureDto>? Measures { get; set; }
        public UnitOfMeasureDto? Dto { get; set; }
    }
}

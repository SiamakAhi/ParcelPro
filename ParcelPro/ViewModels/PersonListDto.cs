using ParcelPro.ViewModels.PartyDto;

namespace ParcelPro.ViewModels
{
    public class PersonListDto
    {
        public Pagination<PersonDto> persen { get; set; }
        public PersonFilterDto filter { get; set; }
    }
}

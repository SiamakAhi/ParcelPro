using ParcelPro.ViewModels;
using ParcelPro.ViewModels.PartyDto;

namespace ParcelPro.Areas.Representatives.Dtos
{
    public class MembersViewmodel
    {
        public PersonFilterDto filter { get; set; } = new PersonFilterDto();

        public Pagination<PersonDto>? Persen { get; set; }
        public PersonDto CreditClient { get; set; }

    }
}

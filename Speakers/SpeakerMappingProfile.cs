using AutoMapper;

namespace AspNet5MongoDb.Speakers
{
    public class SpeakerMappingProfile : Profile
    {
		protected override void Configure() {
			Mapper.CreateMap<SpeakerDto, Speaker>()
				.ForMember(s=>s.Id, mapper=>mapper.Ignore());
			Mapper.CreateMap<Speaker, SpeakerDto>();
		}
    }
}
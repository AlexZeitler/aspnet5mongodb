using AutoMapper;
using AspNet5MongoDb.Speakers;

namespace AspNet5MongoDb.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.AddProfile<SpeakerMappingProfile>();
        }
    }
}
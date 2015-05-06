using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AspNet5MongoDb.Speakers
{


    [Route("api/[controller]")]
    public class SpeakersController
    {

        private readonly IMongoDatabase _database;

        public SpeakersController(IMongoDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var speakers = await _database.GetCollection<Speaker>("speakers").Find(_ => true).ToListAsync();
            return new ObjectResult(Mapper.Map<List<Speaker>, List<SpeakerDto>>(speakers));
        }

        [HttpGet]
        [Route("{id}", Name = "SpeakerDetailsById")]
        public async Task<IActionResult> GetById(string id)
        {
            var speakers = await _database.GetCollection<Speaker>("speakers")
                  .Find(c => c.MongoId == new ObjectId(id))
                  .ToListAsync();
            var speaker = speakers.FirstOrDefault();
            if (speaker == null)
            {
                return new HttpNotFoundResult();
            }

            return new ObjectResult(Mapper.Map<Speaker,SpeakerDto>(speaker));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SpeakerDto speakerDto)
        {
            try {
                var speaker = Mapper.Map<SpeakerDto, Speaker>(speakerDto);
            await _database.GetCollection<Speaker>("speakers").InsertOneAsync(speaker);
            return new CreatedAtRouteResult("SpeakerDetailsById", new { id = speaker.Id }, speaker);
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
                return new HttpStatusCodeResult(500);
            }   
        }
    }
}

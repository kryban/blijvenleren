using BlijvenLeren.Models;
using BlijvenLeren.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlijvenLeren.Controllers
{
    [Authorize]
    [Route("api/blijvenlerenresources")]
    [ApiController]
    public class BlijvenLerenApiController : ControllerBase
    {
        private IBlijvenLerenRepository _repo;

        public BlijvenLerenApiController(IBlijvenLerenRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IEnumerable<LearnResource> Get()
        {
            return _repo.GetAllLearnResources().Result;
        }

        [HttpGet("{id}")]
        public LearnResource Get(int id)
        {
            return _repo.GetLearnResource(id).Result;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.DeleteLearnResource(id);
        }
    }
}

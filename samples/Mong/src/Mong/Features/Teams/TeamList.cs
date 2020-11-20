using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Miru.Mvc;
using Mong.Database;

namespace Mong.Features.Teams
{
    public class TeamList
    {
        public class Query : IRequest<Result>
        {
        }

        public class Result
        {    
            public IReadOnlyList<Item> Items { get; set; } = new List<Item>();
        }

        public class Item
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly MongDbContext _db;
            
            public Handler(MongDbContext db)
            {
                _db = db;
            }
            
            public async Task<Result> Handle(Query request, CancellationToken ct)
            {
                return new Result
                {
                    Items = await _db.Teams
                        .Select(m => new Item
                        {
                            Id = m.Id,
                            Name = m.Name
                        })
                        .ToListAsync(ct)
                };
            }
        }
        
        public class TeamsController : MiruController
        {
            [Route("/Teams")]
            public async Task<Result> List(Query request) => await SendAsync(request);
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoFixture;
using Miru.Pagination;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Pagination
{
    public class PageableTest
    {
        private IQueryable<Item> _items;

        [SetUp]
        public void Setup()
        {
            _items = new Fixture().CreateMany<Item>(20).AsQueryable();
        }

        [Test]
        public void Should_return_if_there_is_previous_page()
        {
            var query = new Query { Page = 1, PageSize = 2 };
            query.Results = _items.ToPaginate(query).ToList();
            query.HasPreviousPage().ShouldBeFalse();

            query.Page = 10;
            query.Results = _items.ToPaginate(query).ToList();
            query.HasPreviousPage().ShouldBeTrue();
        }
        
        [Test]
        public void Should_return_if_there_is_next_page()
        {
            var query = new Query { Page = 10, PageSize = 2 };
            query.Results = _items.ToPaginate(query).ToList();
            query.HasNextPage().ShouldBeFalse();

            query.Page = 1;
            query.Results = _items.ToPaginate(query).ToList();
            query.HasNextPage().ShouldBeTrue();
        }

        [Test]
        public void Should_return_pager()
        {
            // arrange
            var list = new Fixture().CreateMany<Item>(20).AsQueryable();
            var query = new Query
            {
                Page = 5, 
                PageSize = 2
            };
            
            query.Results = list.ToPaginate(query).ToList();
            
            // act & assert
            query.Pager(7).Pages.ShouldBe(new[] { 2, 3, 4, 5, 6, 7, 8 });
            query.Pager(5).Pages.ShouldBe(new[] { 3, 4, 5, 6, 7 });
            query.Pager(3).Pages.ShouldBe(new[] { 4, 5, 6 });
            query.Pager(1).Pages.ShouldBe(new[] { 5 });
            query.Pager(20).Pages.ShouldBe(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
        }
        
        public class Query : IPageable<Item>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int Pages { get; set; }
            public int CountShowing { get; set; }
            public int CountTotal { get; set; }
            
            public IReadOnlyList<Item> Results { get; set; } = new Collection<Item>();
        }

        public class Item
        {
        }
    }
}
using FluentAssertions;
using ShareThings.Domain;
using System;
using Xunit;

namespace ShareThings.UnitTest.Domain
{
    public class TermDomainTest
    {
        [Fact]
        public void GivenTermWhenStartIsGreaterThanEndThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Term(DateTime.Now.AddDays(5), DateTime.Now.AddDays(-5)));
        }

        [Fact]
        public void GivenTermWhenIsTermIncludeThenResultsOk()
        {
            Term term = new Term(DateTime.Now.AddDays(-5), DateTime.Now.AddDays(5));
            term.IsTermInclude(new Term(DateTime.Now.AddDays(-5), term.End)).Should().BeTrue();
            term.IsTermInclude(new Term(DateTime.Now.AddDays(-6), term.End)).Should().BeFalse();
            term.IsTermInclude(new Term(DateTime.Now.AddDays(-1), DateTime.Now)).Should().BeTrue();
            term.IsTermInclude(new Term(DateTime.Now.AddDays(-6), DateTime.Now.AddDays(6))).Should().BeFalse();
            term.IsTermInclude(new Term(term.Start, term.End)).Should().BeTrue();
            term.IsTermInclude(new Term(term.Start, DateTime.Now.AddDays(4))).Should().BeTrue();
            term.IsTermInclude(new Term(term.Start, DateTime.Now.AddDays(6))).Should().BeFalse();
        }
    }
}
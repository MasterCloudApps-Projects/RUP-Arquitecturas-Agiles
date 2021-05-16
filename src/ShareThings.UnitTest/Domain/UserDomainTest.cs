using FluentAssertions;
using ShareThings.Domain;
using System;
using Xunit;

namespace ShareThings.UnitTest.Domain
{
    public class UserDomainTest
    {
        [Fact]
        public void GivenUserWhenUnRegisterThenUserIdentityIdIsEmpty()
        {
            string UserIdentityId = Guid.NewGuid().ToString();
            User lender = new User(UserIdentityId);
            lender.UserIdentityId.Should().Be(UserIdentityId);
            lender.UnRegister();
            lender.UserIdentityId.Should().BeEmpty();
        }
    }
}
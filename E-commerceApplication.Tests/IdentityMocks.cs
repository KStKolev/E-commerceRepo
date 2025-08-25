﻿using Microsoft.AspNetCore.Identity;
using Moq;

namespace E_commerceApplication.Tests
{
    public static class IdentityMocks
    {
        public static Mock<UserManager<TUser>> MockUserManager<TUser>() 
            where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();

            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        public static Mock<SignInManager<TUser>> MockSignInManager<TUser>(Mock<UserManager<TUser>> userManager) 
            where TUser : class
        {
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();

            return new Mock<SignInManager<TUser>>(
                userManager.Object,
                contextAccessor.Object,
                claimsFactory.Object,
                null,
                null,
                null,
                null);
        }
    }
}

using ShareThings.Areas.Identity.Data;
using System;

namespace ShareThings.FunctionalTest.Authorization
{
    public sealed class IdentitySingleton
    {
        public const string KeyBorrower = "TestBorrower";
        public const string KeyLender = "TestLender";

        private readonly static IdentitySingleton _instance = new IdentitySingleton();
        private readonly ShareThingsUser _userBorrower;
        private readonly ShareThingsUser _userLender;

        private IdentitySingleton() 
        {
            this._userBorrower = new ShareThingsUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = KeyBorrower,
                Email = KeyBorrower + "@sharethings.es"
            };

            this._userLender = new ShareThingsUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = KeyLender,
                Email = KeyLender + "@sharethings.es"
            };
        }

        public static IdentitySingleton Instance
        {
            get
            {
                return _instance;
            }
        }

        public ShareThingsUser GetBorrower()
        {
            return this._userBorrower;
        }

        public ShareThingsUser GetLender()
        {
            return this._userLender;
        }
    }
}

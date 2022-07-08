using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        List<User> Users = new List<User>()
        {
        //    new User()
        //    {
        //        FirstName="Read Only",LastName="User",EmailAddress="readonly@user.com" ,
        //        Username="readonly@user.com", Password="Readonly@user",Id=Guid.NewGuid(),Roles=new List<string> {"Reader"}
        //    },
        //    new User()
        //    {
        //        FirstName="Read Write",LastName="User",EmailAddress="readwrite@user.com" ,
        //        Username="readwrite@user.com", Password="Readwrite@user",Id=Guid.NewGuid(),Roles=new List<string> {"Reader","Writer"}
        //    }
        };

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = Users.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
            x.Password == password);

            return  user;
        }
    }
}

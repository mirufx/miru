using Miru.Userfy;

namespace Pantanal.Domain
{
    public class User : UserfyUser
    {
        public override string Display => Email;
    }
}

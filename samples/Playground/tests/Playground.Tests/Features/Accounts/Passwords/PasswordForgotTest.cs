using System.Threading.Tasks;
using Miru.Testing;
using NUnit.Framework;
using Playground.Domain;
using Playground.Features.Accounts.Passwords;
using Shouldly;

namespace Playground.Tests.Features.Accounts.Passwords;

public class PasswordForgotTest : FeatureTest
{
    [Test]
    public async Task Can_generate_reset_email()
    {
        // arrange
        var user = await _.MakeUserAsync<User>();

        // act
        await _.SendAsync(new PasswordForgot.Command
        {
            Email = user.Email
        });

        // assert
        var job = _.LastEmailSent();
        job.Data.ToAddresses.ShouldContainEmail(user.Email);
        job.Data.Body.ShouldContain("password reset");
    }

    public class Validations : ValidationTest<PasswordForgot.Command>
    {
        [Test]
        public void Email_is_required_and_valid()
        {
            ShouldBeValid(Request, m => m.Email, Request.Email);

            ShouldBeInvalid(Request, m => m.Email, string.Empty);
            ShouldBeInvalid(Request, m => m.Email, "admin!.admin");
        }
    }
}
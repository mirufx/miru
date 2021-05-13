using Miru.Testing;
using NUnit.Framework;

namespace Pantanal.PageTests
{
    [SetUpFixture]
    public class Program
    {
        public static int Main(string[] args) => new TestRunner().RunAssemblyOfType<Program>(args);

        [OneTimeTearDown]
        public void Dispose() => TestMiruHost.ExecuteAfterSuite();
    }
}

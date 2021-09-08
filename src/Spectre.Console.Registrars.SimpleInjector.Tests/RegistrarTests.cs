using Shouldly;

using Xunit;

namespace Spectre.Console.Registrars.SimpleInjector.Tests
{
    public class RegistrarTests
    {
        [Fact]
        public void Resolver_Should_Return_Registration_From_Container()
        {
            var fixture = new RegistrarFixture();
            fixture.GivenOnContainer(c => c.Register<ISomeInterface, SomeDependency>());

            var actual = fixture.GetResolver().Resolve(typeof(ISomeInterface));

            actual.ShouldNotBeNull();
            actual.ShouldBeOfType<SomeDependency>();
        }

        [Fact]
        public void Resolver_Should_Return_Registration_From_Registrar()
        {
            var fixture = new RegistrarFixture();
            fixture.GivenOnRegistrar(r => r.Register(typeof(ISomeInterface), typeof(SomeDependency)));

            var actual = fixture.GetResolver().Resolve(typeof(ISomeInterface));

            actual.ShouldNotBeNull();
            actual.ShouldBeOfType<SomeDependency>();
        }

        [Fact]
        public void Resolver_Should_Return_Instance_From_Registrar()
        {
            var fixture = new RegistrarFixture();
            var expected = new SomeDependency();
            fixture.GivenOnRegistrar(r => r.RegisterInstance(typeof(ISomeInterface), expected));

            var actual = fixture.GetResolver().Resolve(typeof(ISomeInterface));

            actual.ShouldNotBeNull();
            ReferenceEquals(expected, actual).ShouldBeTrue();
        }

        [Fact]
        public void Resolver_Should_Return_Lazy_From_Registrar()
        {
            var fixture = new RegistrarFixture();
            var expected = new SomeDependency();
            fixture.GivenOnRegistrar(r => r.RegisterLazy(typeof(ISomeInterface), () => expected));

            var actual = fixture.GetResolver().Resolve(typeof(ISomeInterface));

            actual.ShouldNotBeNull();
            ReferenceEquals(expected, actual).ShouldBeTrue();
        }

        [Fact(Skip = "Doesn't work at the moment")]
        public void Resolver_Should_Not_Call_Lazy_Factory_If_Not_Needed()
        {
            var fixture = new RegistrarFixture();
            var factoryCalled = false;
            fixture.GivenOnRegistrar(r => r.RegisterLazy(typeof(ISomeInterface), () =>
            {
                factoryCalled = true;
                return new SomeDependency();
            }));

            fixture.GetResolver();

            factoryCalled.ShouldBeFalse();
        }

        private interface ISomeInterface
        {
        }

        private class SomeDependency : ISomeInterface
        {
        }

        private class SomeOtherDependency : ISomeInterface
        {
        }
    }
}

using System;

using Shouldly;

using Xunit;

namespace Spectre.Console.Registrars.SimpleInjector.Tests
{
    public class RegistrarTests
    {
        [Fact]
        public void Resolver_Resolving_null_Throws()
        {
            var fixture = new RegistrarFixture();
            var resolver = fixture.GetResolver();

            Action action = () => resolver.Resolve(null);

            action.ShouldThrow<ArgumentNullException>();
        }

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

        [Fact]
        public void Resolver_Resolving_From_Multiple_Lazies_Returns_The_Last_Registration()
        {
            var fixture = new RegistrarFixture();
            fixture.GivenMultiRegistrationTypes(typeof(ISomeInterface));
            fixture.GivenOnRegistrar(r => r.RegisterLazy(typeof(ISomeInterface), () => new SomeDependency()));
            fixture.GivenOnRegistrar(r => r.RegisterLazy(typeof(ISomeInterface), () => new SomeOtherDependency()));

            var actual = fixture.GetResolver().Resolve(typeof(ISomeInterface));

            actual.ShouldNotBeNull();
            actual.ShouldBeOfType<SomeOtherDependency>();
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

        [Fact]
        public void Resolver_Registering_Multiple_Does_Not_Throw()
        {
            var fixture = new RegistrarFixture();
            fixture.GivenMultiRegistrationTypes(typeof(ISomeInterface));
            fixture.GivenOnRegistrar(r => r.Register(typeof(ISomeInterface), typeof(SomeDependency)));
            fixture.GivenOnRegistrar(r => r.Register(typeof(ISomeInterface), typeof(SomeOtherDependency)));

            var actual = fixture.GetResolver().Resolve(typeof(ISomeInterface));

            actual.ShouldNotBeNull();
        }

        [Fact]
        public void Resolver_Resolving_From_Multiple_Returns_The_Last_Registration()
        {
            var fixture = new RegistrarFixture();
            fixture.GivenMultiRegistrationTypes(typeof(ISomeInterface));
            fixture.GivenOnRegistrar(r => r.Register(typeof(ISomeInterface), typeof(SomeDependency)));
            fixture.GivenOnRegistrar(r => r.Register(typeof(ISomeInterface), typeof(SomeOtherDependency)));

            var actual = fixture.GetResolver().Resolve(typeof(ISomeInterface));

            actual.ShouldNotBeNull();
            actual.ShouldBeOfType<SomeOtherDependency>();
        }

        [Fact]
        public void Resolver_Resolving_From_Multiple_Instances_Returns_The_Last_Registration()
        {
            var fixture = new RegistrarFixture();
            fixture.GivenMultiRegistrationTypes(typeof(ISomeInterface));
            fixture.GivenOnRegistrar(r => r.RegisterInstance(typeof(ISomeInterface), new SomeDependency()));
            fixture.GivenOnRegistrar(r => r.RegisterInstance(typeof(ISomeInterface), new SomeOtherDependency()));

            var actual = fixture.GetResolver().Resolve(typeof(ISomeInterface));

            actual.ShouldNotBeNull();
            actual.ShouldBeOfType<SomeOtherDependency>();
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

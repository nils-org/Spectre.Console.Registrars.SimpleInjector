using System;
using SimpleInjector;
using Spectre.Console.Cli;

namespace Spectre.Console
{
    /// <summary>
    /// Implements <see cref="ITypeRegistrar"/> using a SimpleInjector <see cref="Container"/>.
    /// </summary>
    public class SimpleInjectorRegistrar : ITypeRegistrar
    {
        private readonly Container container;
        private readonly Lifestyle lifestyle;

        /// <summary>
        /// Constructs a new instance using the the given <see cref="Container" />.
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to build the <see cref="ITypeRegistrar"/> around.</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> to use during registrations. Default is <see cref="Lifestyle.Singleton"/>.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SimpleInjectorRegistrar(Container container,
            Lifestyle lifestyle = null)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
            this.lifestyle = lifestyle ?? Lifestyle.Singleton;
        }

        /// <inheritdoc cref="ITypeRegistrar.Register"/>
        public void Register(Type service, Type implementation)
        {
            container.Register(service, implementation, lifestyle);
        }

        /// <inheritdoc cref="ITypeRegistrar.RegisterInstance"/>
        public void RegisterInstance(Type service, object implementation)
        {
            container.RegisterInstance(service, implementation);
        }

        /// <inheritdoc cref="ITypeRegistrar.RegisterLazy"/>
        public void RegisterLazy(Type service, Func<object> factory)
        {
            // todo: non of these code-paths are lazy!!
            container.Register(service, factory, lifestyle);
        }

        /// <inheritdoc cref="ITypeRegistrar.Build"/>
        public ITypeResolver Build()
        {
            container.Verify();
            return new SimpleInjectorTypeResolver(container);
        }

        private class SimpleInjectorTypeResolver : ITypeResolver
        {
            private readonly Container container;

            public SimpleInjectorTypeResolver(Container container)
            {
                this.container = container;
            }

            public object Resolve(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                var implementation = container.GetInstance(type);

                return implementation;
            }
        }
    }
}

using System;
using System.Linq;
using SimpleInjector;
using Spectre.Console.Cli;

namespace Spectre.Console.Registrars.SimpleInjector
{
    /// <summary>
    /// Implements <see cref="ITypeRegistrar"/> using a SimpleInjector <see cref="Container"/>.
    /// </summary>
    public class SimpleInjectorRegistrar : ITypeRegistrar
    {
        private readonly Container container;
        private readonly Lifestyle lifestyle;
        private readonly Type[] multiRegistrationTypes;

        /// <summary>
        /// Constructs a new instance using the the given <see cref="Container" />.
        /// <para>
        /// Since SimpleInjector inherently
        /// <see href="https://docs.simpleinjector.org/en/latest/decisions.html#separate-collections">differentiates
        /// the registration of collections from registering a single implementation</see> the types that are to be
        /// registered as collections need to be known before registering them. A list of types that are registered
        /// as collections can be set using the <paramref name="multiRegistrationTypes"/> param.
        /// </para>
        /// </summary>
        /// <param name="container">The <see cref="Container"/> to build the <see cref="ITypeRegistrar"/> around.</param>
        /// <param name="lifestyle">The <see cref="Lifestyle"/> to use during registrations. Default is <see cref="Lifestyle.Singleton"/>.</param>
        /// <param name="multiRegistrationTypes">List of types that are to be registered multiple times.
        /// Default is [ <see cref="ICommand{TSettings}"/>, <see cref="ICommand"/> ].</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SimpleInjectorRegistrar(Container container,
            Lifestyle lifestyle = null,
            Type[] multiRegistrationTypes = null)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
            this.lifestyle = lifestyle ?? Lifestyle.Singleton;
            this.multiRegistrationTypes = multiRegistrationTypes ?? new[]
            {
                typeof(ICommand),
                typeof(ICommand<>)
            };
        }

        /// <inheritdoc cref="ITypeRegistrar.Register"/>
        public void Register(Type service, Type implementation)
        {
            if (multiRegistrationTypes.Contains(service))
            {
                container.Collection.Append(service, implementation, lifestyle);
                return;
            }

            container.Register(service, implementation, lifestyle);
        }

        /// <inheritdoc cref="ITypeRegistrar.RegisterInstance"/>
        public void RegisterInstance(Type service, object implementation)
        {
            if (multiRegistrationTypes.Contains(service))
            {
                container.Collection.AppendInstance(service, implementation);
                return;
            }

            container.RegisterInstance(service, implementation);
        }

        /// <inheritdoc cref="ITypeRegistrar.RegisterLazy"/>
        public void RegisterLazy(Type service, Func<object> factory)
        {
            // todo: non of these code-paths are lazy!!
            if (multiRegistrationTypes.Contains(service))
            {
                // todo: this ignores the lifestyle, but seemingly Func<object> can not be used on collections.
                container.Collection.AppendInstance(service, factory());
                return;
            }

            container.Register(service, factory, lifestyle);
        }

        /// <inheritdoc cref="ITypeRegistrar.Build"/>
        public ITypeResolver Build()
        {
            container.Verify();
            return new SimpleInjectorTypeResolver(container, multiRegistrationTypes);
        }

        private class SimpleInjectorTypeResolver : ITypeResolver
        {
            private readonly Container container;
            private readonly Type[] multiRegistrationTypes;

            public SimpleInjectorTypeResolver(Container container, Type[] multiRegistrationTypes)
            {
                this.container = container;
                this.multiRegistrationTypes = multiRegistrationTypes;
            }

            public object Resolve(Type type)
            {
                if (type == null)
                {
                    throw new ArgumentNullException(nameof(type));
                }

                if (multiRegistrationTypes.Contains(type))
                {
                    return container.GetAllInstances(type).LastOrDefault();
                }

                return container.GetInstance(type);
            }
        }
    }
}

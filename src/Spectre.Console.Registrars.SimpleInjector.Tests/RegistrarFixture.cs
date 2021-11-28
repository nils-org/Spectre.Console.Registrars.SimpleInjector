using System;
using System.Collections.Generic;

using SimpleInjector;

using Spectre.Console.Cli;

namespace Spectre.Console.Registrars.SimpleInjector.Tests
{
    internal class RegistrarFixture
    {
        private readonly List<Action<Container>> containerActions = new List<Action<Container>>();
        private readonly List<Action<ITypeRegistrar>> registrarActions = new List<Action<ITypeRegistrar>>();
        private Lifestyle lifestyle;

        internal void GivenLifestyle(Lifestyle lifestyle)
        {
            this.lifestyle = lifestyle;
        }

        internal void GivenOnContainer(Action<Container> action)
        {
            containerActions.Add(action);
        }

        internal void GivenOnRegistrar(Action<ITypeRegistrar> action)
        {
            registrarActions.Add(action);
        }

        internal ITypeResolver GetResolver()
        {
            var container = new Container();
            foreach (var action in containerActions)
            {
                action(container);
            }

            var registrar = new SimpleInjectorRegistrar(container, lifestyle);
            foreach (var action in registrarActions)
            {
                action(registrar);
            }

            return registrar.Build();
        }
    }
}

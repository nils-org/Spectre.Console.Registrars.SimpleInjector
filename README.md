# Spectre.Console.Registrars.SimpleInjector

[![standard-readme compliant][]][standard-readme]
[![Contributor Covenant][contrib-covenantimg]][contrib-covenant]
[![Build][githubimage]][githubbuild]
[![Codecov Report][codecovimage]][codecov]
[![NuGet package][nugetimage]][nuget]

An implementation of an ITypeRegistrar for Spectre.Console using SimpleInjector

**This library is deprecated, SimpleInjector can not fulfilly all required features
to implement an ITypeRegistrar!**

## Table of Contents

- [Install](#install)
- [Usage](#usage)
- [Maintainer](#maintainer)
- [Contributing](#contributing)
- [License](#license)

## Install

```ps
Install-Package Spectre.Console.Registrars.SimpleInjector
```

## Usage

```
var container = new SimpleInjector.Container();
// setup your end of the container...
var registrar = new SimpleInjectorRegistrar(container);
var app = new CommandApp(registrar);
```

## Maintainer

[Nils Andresen @nils-a][maintainer]

## Contributing

Spectre.Console.Registrars.SimpleInjector follows the [Contributor Covenant][contrib-covenant] Code of Conduct.

We accept Pull Requests.

Small note: If editing the Readme, please conform to the [standard-readme][] specification.

## License

[MIT License © Nils Andresen][license]

[githubbuild]: https://github.com/nils-org/Spectre.Console.Registrars.SimpleInjector/actions/workflows/build.yml?query=branch%3Adevelop
[githubimage]: https://github.com/nils-org/Spectre.Console.Registrars.SimpleInjector/actions/workflows/build.yml/badge.svg?branch=develop
[codecov]: https://codecov.io/gh/nils-org/Spectre.Console.Registrars.SimpleInjector
[codecovimage]: https://img.shields.io/codecov/c/github/nils-org/Spectre.Console.Registrars.SimpleInjector.svg?logo=codecov&style=flat-square
[contrib-covenant]: https://www.contributor-covenant.org/version/2/0/code_of_conduct/
[contrib-covenantimg]: https://img.shields.io/badge/Contributor%20Covenant-v2.0%20adopted-ff69b4.svg
[maintainer]: https://github.com/nils-a
[nuget]: https://nuget.org/packages/Spectre.Console.Registrars.SimpleInjector
[nugetimage]: https://img.shields.io/nuget/v/Spectre.Console.Registrars.SimpleInjector.svg?logo=nuget&style=flat-square
[license]: LICENSE.txt
[standard-readme]: https://github.com/RichardLitt/standard-readme
[standard-readme compliant]: https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square
[documentation]: https://nils-org.github.io/Spectre.Console.Registrars.SimpleInjector/
[api]: https://cakebuild.net/api/Cake.SevenZip/

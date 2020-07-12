<p align="center">
<!-- PROJECT LOGO -->
<img src="palantir_small.png" alt="Palantir Web API">
<br />
  <h2 align="center">Palantir project - Web API</h2>

  <p align="center">
    The web API of the Palantir Information Radiator Project
    <br />
    <a href="https://gitlab.com/untref-ingsoft/tfi-cozzi/information-radiator/-/wikis/home"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="http://ec2-18-230-21-194.sa-east-1.compute.amazonaws.com:8080/">View Demo</a>
    ·
    <a href="https://gitlab.com/untref-ingsoft/tfi-cozzi/information-radiator/issues">Report Bug</a>
    ·
    <a href="https://gitlab.com/untref-ingsoft/tfi-cozzi/information-radiator/issues">Request Feature</a>
  </p>
</p>

<!-- TABLE OF CONTENTS -->

## Table of Contents

- [About the Project](#about-the-project)
  - [Built And Tested With](#built-and-tested-with)
  - [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Installation (with the Palantir web UI)](#installation-with-the-palantir-web-ui)
- [Usage](#usage)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)
- [Additional bibliography and acknowledgements](#additional-bibliography-and-acknowledgements)

<!-- ABOUT THE PROJECT -->

## About The Project

Web API developed in .NET Core for the Palantir Information Radiator project. Main objective: to communicate with various online administration and monitoring services of software development projects, such as GitHub, Pivotal Tracker, GitLab, among others, whose data will be displayed in the [Palantir web UI](https://gitlab.com/untref-ingsoft/tfi-cozzi/palantir-tizen-ui) on Tizen OS Smart TVs and another devices as _single-board computers_ such as Raspberry Pi.

### Built And Tested With

- [.NET Core](https://dotnet.microsoft.com)
- [xUnit.net](https://xunit.net/)

<!-- GETTING STARTED -->

## Getting Started

To get a local copy up and running consider these recommendations and follow these simple example steps:

### Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop)
- [Docker Compose](https://docs.docker.com/compose/install/) (for use with the [Palantir web UI project](https://gitlab.com/untref-ingsoft/tfi-cozzi/palantir-tizen-ui))
- [.NET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) (for developing purposes only)

### Installation

1. Clone the repo

```sh
git clone https://github.com/untref-ingsoft/tfi-cozzi/information-radiator.git
```

2. Go to the project root folder
3. Build the project

```sh
dotnet build
```

4. Go to the Palantir.WebApi assembly folder inside the root folder
5. Run the project

```sh
dotnet run
```

By default, the .NET Core project will run on a Kestrel web server local instance, whose default port is 5000.

If you are not interested in developing the project, but just using the API, you can download it from Docker Hub with the following command:

- For AMD64 (x86-64) Docker installations:

```sh
docker pull gonzalocozzi/palantir-api-amd64
```

- For ARM64 (aarch64) Docker installations:

```sh
docker pull gonzalocozzi/palantir-api-arm64
```

...and run the Docker container with:

- For AMD64 (x86-64) Docker installations (with port 5000 exposed):

```sh
docker run -d -p 5000:80 gonzalocozzi/palantir-api-amd64
```

- For ARM64 (aarch64) Docker installations (with port 5000 exposed):

```sh
docker run -d -p 5000:80 gonzalocozzi/palantir-api-arm64
```

You can also generate the Docker image locally, after cloning the repository, from the Dockerfile available in the project:

```sh
docker build -t docker-image-name path-to-dockerfile
```

### Installation (with the Palantir web UI)

You can use this project in conjunction with the [Palantir web UI project](https://gitlab.com/untref-ingsoft/tfi-cozzi/palantir-tizen-ui), using Docker Compose. This requires three files **which must be located in the same folder**:

- a configuration file for this API, called _apiConfig.json_ (example [at this link](https://gitlab.com/untref-ingsoft/tfi-cozzi/information-radiator/-/blob/master/deploy/apiConfig.json))

- a configuration file for the web frontend called _webConfig.json_ (example [at this link](https://gitlab.com/untref-ingsoft/tfi-cozzi/palantir-tizen-ui/-/blob/master/deploy/webConfig.json))

- a Docker Compose file (example [at this link](https://gitlab.com/untref-ingsoft/tfi-cozzi/information-radiator/-/blob/master/deploy/docker-compose.yml)).

To run the Docker Compose and start the application, the following command must be run in the folder in which the three files are stored:

```sh
docker-compose up -d
```

<!-- USAGE EXAMPLES -->

## Usage as administrator (ADMIN)

Initially, you need to set up the services that will return information about the development project(s) you want to follow up:

- Configuring a _Source Control Management_ (SCM) service:

```sh
curl -X POST -d {"service":"Palantir.ServiceAssembly","user":"user-name","projects":["project-name"],"token":"alphanumeric-token"} -H 'Content-Type: application/json' http://localhost:5000/admin/scm
```

- Configuring a _Issue Tracking_ (IT) service:

```sh
curl -X POST -d {"service":"Palantir.ServiceAssembly","projects":["project-name"],"token":"alphanumeric-token"} -H 'Content-Type: application/json' http://localhost:5000/admin/it
```

- Configuring an _Build Server_ (BS) service:

```sh
curl -X POST -d {"service":"Palantir.ServiceAssembly","projects":["project-number"],"token":"alphanumeric-token"} -H 'Content-Type: application/json' http://localhost:5000/admin/bs
```

- Configuring an _Iframe_ service:

```sh
curl -X POST -d {"Iframe": "<iframe src='ifreame-address' style='border: 0' width='600' height='600' frameborder='0' scrolling='no'></iframe>"
} -H 'Content-Type: application/json' http://localhost:5000/admin/iframe
```

All this settings will be stored in the API configuration file (_apiConfig.json_) previously mentioned.

You can then ask the API for the stored settings, for example from the _Source Control Manager_ (SCM).

```sh
curl -X GET http://localhost:5000/admin/scm
```

You can also update the stored setting (only if there is a previously stored setting for the service) or delete it:

- Updating the _Source Control Manager_ (SCM) settings:

```sh
curl -X PUT -d {"service":"Palantir.GitHub","user":"user-name","projects":["project-name"],"token":"alphanumeric-token"}} -H {{'Content-Type: application/json'} http://localhost:5000/admin/scm
```

- Deleting the _Source Control Manager_ (SCM) settings:

```sh
curl -X DELETE http://localhost:5000/admin/scm
```

## Usage as user (VIEW)

As a user, you will be able to obtain the data that the monitoring services offer. To do this, you can request them from the API:

- Getting data from a _Source Control Manager_ (SCM) service:

```sh
curl -X GET http://localhost:5000/view/scm/
```

- Getting data from a _Issue Tracking_ (IT) service:

```sh
curl -X GET http://localhost:5000/view/it/
```

- Getting data from a _Build Server_ (BS) service:

```sh
curl -X GET http://localhost:5000/view/bs/
```

<!-- ROADMAP -->

## Roadmap

See the [open issues](https://gitlab.com/untref-ingsoft/tfi-cozzi/information-radiator/issues) for a list of proposed features (and known issues).

<!-- CONTRIBUTING -->

## Contributing

Please read [CONTRIBUTING](https://gitlab.com/untref-ingsoft/tfi-cozzi/information-radiator/-/blob/master/CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

<!-- LICENSE -->

## License

Distributed under the GPL License. See [LICENSE](https://gitlab.com/untref-ingsoft/tfi-cozzi/information-radiator/-/blob/master/LICENSE) for more information.

<!-- CONTACT -->

## Contact

Gonzalo Alejandro Cozzi - gcozzi@untref.edu.ar

<!-- ACKNOWLEDGEMENTS -->

## Additional bibliography and acknowledgements

- [Scrum Patterns](http://scrumbook.org/value-stream/information-radiator.html)
- [Agile Software Development: Forming Teams that Communicate and Cooperate](http://www.informit.com/articles/article.aspx?p=24486)
- [Information radiator](http://josehuerta.es/gestion/agile/information-radiators/information-radiator)
- [Tutorial: Create a web API with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.0&tabs=visual-studio-code)
- [Creating Web API in ASP.NET Core 2.0](https://www.codingame.com/playgrounds/35462/creating-web-api-in-asp-net-core-2-0/)
- [Unit Testing in ASP.NET Core Web API](https://code-maze.com/unit-testing-aspnetcore-web-api/)
- [.Net Core Unit Test and Code Coverage with Visual Studio Code](https://dev.to/deinsoftware/net-core-unit-test-and-code-coverage-with-visual-studio-code-37bp)
- [Docker images for ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1)
- [Build and run your Docker image](https://docs.docker.com/get-started/part2/)
- [Share images on Docker Hub](https://docs.docker.com/get-started/part3/)
- [Overview of Docker Compose](https://docs.docker.com/compose/)
- [Choose an Open Source License](https://choosealicense.com)

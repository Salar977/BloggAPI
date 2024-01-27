# BloggAPI

Brief description of your API.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Configuration for MySQL database](#configuration)
- [Usage](#usage)

## Getting Started

### Prerequisites

- Visual Studio 2022
- MySQL 8.0

### Installation

1. git clone https://github.com/Salar977/BloggAPI.git
2. run BloggAPI.sln

## Configuration for MySQL database

1. open cmd and go inside BloggAPI/src/Blogg.API
2. **dotnet ef migrations add initialBlogg -o Data/Migrations**
3. **dotnet ef database update**

## Usage

After going through Installation and Configuration, then just run the program
The api uses authentication so a user has to be created before it can use the rest of the api.
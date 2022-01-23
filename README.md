# passchamp

[![CICD](https://github.com/devoctomy/passchamp/actions/workflows/cicd.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/cicd.yml)
[![codecov](https://codecov.io/gh/devoctomy/passchamp/branch/main/graph/badge.svg?token=JU70OAK6OX)](https://codecov.io/gh/devoctomy/passchamp)
[![Codacy Security Scan](https://github.com/devoctomy/passchamp/actions/workflows/codacy-analysis.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/codacy-analysis.yml)

The intention of passchamp is to provide an entirely dotnet 6 rewrite of [cachy](https://github.com/devoctomy/cachy) and also provide a framework for building additional clients that can utilise the file format, or build new ones.

## Components

### Core

Class library containing all core components of passchamp, this will be used to build all clients upon.

### SignTool

Console application used for signing JSON files using an RSA key pair.

The intention of this is to protect graph json files, as potentially, a bad actor could manipulate a graph and caused it to do undesirable things.  Or the graph could just be bug ridden, so the admin needs to know that the one they have configured is safe/official.

This will be achieved via digital signatures and verification of said signatures within the host application.
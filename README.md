# passchamp

## Current branch badges

> Not available yet

## Main branch badges

[![CICD](https://github.com/devoctomy/passchamp/actions/workflows/cicd.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/ciccd.yml)
[![codecov](https://codecov.io/gh/devoctomy/passchamp/branch/main/graph/badge.svg?token=JU70OAK6OX)](https://codecov.io/gh/devoctomy/passchamp)
[![Codacy Security Scan](https://github.com/devoctomy/passchamp/actions/workflows/codacy-analysis.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/codacy-analysis.yml)

---

passchamp is a cross-platform password management application, written entirely in .net 6 with the intention of producing a powerful and reusable framework for quickly bulding other graph-based applications. 

## Components

### Client

.net MAUI Client for the following platforms.

#### Android

#### iOS

> Build failure currently being ignored in CICD pipeline.

#### MacCatalyst

#### Windows

### Core

Class library containing all core components of passchamp, this will be used to build all clients upon.

### Maui

Class library containing Maui platform specific implementations. This is used directly by Client.

### SignTool

Console application used for signing JSON files using an RSA key pair.

The intention of this is to protect graph json files, as potentially, a bad actor could manipulate a graph and caused it to do undesirable things.  Or the graph could just be bug ridden, so the admin needs to know that the one they have configured is safe/official.

This will be achieved via digital signatures and verification of said signatures within the host application.
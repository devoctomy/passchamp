# passchamp

[![CICD](https://github.com/devoctomy/passchamp/actions/workflows/cicd.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/ciccd.yml)
[![Codecov Badge](https://codecov.io/gh/devoctomy/passchamp/branch/main/graph/badge.svg?token=JU70OAK6OX)](https://codecov.io/gh/devoctomy/passchamp)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/d35dea8ab5944b2499fdac865e340406)](https://www.codacy.com/gh/devoctomy/passchamp/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=devoctomy/passchamp&amp;utm_campaign=Badge_Grade)

---

passchamp is a cross-platform password management application, written entirely in .net 8 with the intention of producing a powerful and reusable framework for quickly buulding other crypto graph-based applications. 

## Components

### Client

.net MAUI Client for the following platforms.

#### Android (SDK 21)

#### Windows (10.0.17763.0 >)

### Core

Class library containing all core components of passchamp, this will be used to build all clients upon.

### Maui

Class library containing Maui platform specific implementations. This is used directly by Client.

### SignTool

Console application used for signing JSON files using an RSA key pair.

The intention of this is to protect graph json files, as potentially, a bad actor could manipulate a graph and caused it to do undesirable things.  Or the graph could just be bug ridden, so the admin needs to know that the one they have configured is safe/official.

This will be achieved via digital signatures and verification of said signatures within the host application.

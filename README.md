# passchamp

## Pipelines

[![Unit Tests & Code Coverage](https://github.com/devoctomy/passchamp/actions/workflows/unit-tests.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/unit-tests.yml)
[![Windows MAUI Build](https://github.com/devoctomy/passchamp/actions/workflows/windows-maui.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/windows-maui.yml)
[![Android MAUI Build](https://github.com/devoctomy/passchamp/actions/workflows/android-maui.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/android-maui.yml)
[![MAC MAUI Build](https://github.com/devoctomy/passchamp/actions/workflows/mac-maui.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/mac-maui.yml)
[![IOS MAUI Build](https://github.com/devoctomy/passchamp/actions/workflows/ios-maui.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/ios-maui.yml)

## Coverage & Quality

[![codecov](https://codecov.io/gh/devoctomy/passchamp/branch/main/graph/badge.svg?token=JU70OAK6OX)](https://codecov.io/gh/devoctomy/passchamp)
[![Codacy Security Scan](https://github.com/devoctomy/passchamp/actions/workflows/codacy-analysis.yml/badge.svg)](https://github.com/devoctomy/passchamp/actions/workflows/codacy-analysis.yml)

---

passchamp is a cross-platform password management application, written entirely in .net 6 with the intention of producing a powerful and reusable framework for quickly bulding other graph-based applications. 

## Components

### Client

.net MAUI Client.

### Core

Class library containing all core components of passchamp, this will be used to build all clients upon.

### SignTool

Console application used for signing JSON files using an RSA key pair.

The intention of this is to protect graph json files, as potentially, a bad actor could manipulate a graph and caused it to do undesirable things.  Or the graph could just be bug ridden, so the admin needs to know that the one they have configured is safe/official.

This will be achieved via digital signatures and verification of said signatures within the host application.
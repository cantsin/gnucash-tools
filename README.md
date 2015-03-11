# GNUCash tools

This repository holds various scripts I use with [GNUCash](http://gnucash.org/), mostly to convert to [ledger](http://www.ledger-cli.org/). Please note that the code assumes that the name of the gnucash file you wish to process is `personal.gnucash`; this file cannot be gzipped.

## Installation (Linux)

Install `fsharp` and `nuget` using your distribution's package system.

If you get `Error: SendFailure (Error writing headers)` while running `nuget`, then you will have to import trusted root certificates from [Mozilla's LXR](http://lxr.mozilla.org/seamonkey/source/security/nss/lib/ckfw/builtins/certdata.txt).

    $ mozroots --import --sync

If you still have problems, try:

    $ certmgr -ssl https://go.microsoft.com
    $ certmgr -ssl https://nugetgallery.blob.core.windows.net
    $ certmgr -ssl https://nuget.org

To install dependencies:

    $ nuget install FSharp.Data

To build, specify the path to the FSharp.Data dll:

    $ fsharpc -I FSharp.Data.2.1.1/lib/net40/ provider.fsx

(To run, use `fsharpi` instead.) If you get errors, the gnucash file is likely gzipped! If not, please open a github issue.

## Makefile

A `Makefile` is provided to generate a ledger from a given gunzipped gnucash file. Please edit the variable LEDGERFILE to point to the ledger file name you want.

You can also use the `Makefile` to run ledger reports.

Currently the Makefile only has three reports: `balance`, `cashflow`, `networth`. Those do what you expect, but note that `balance` and `cashflow` only look at the last month.

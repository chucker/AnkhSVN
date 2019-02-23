# AnkhSVN - Subversion support for Visual Studio

(The original readme can still be found at **README.md**, but was woefully outdated)

This fork likely breaks compat with old versions of VS. It is intended for VS 2019, but will likely work with 2017 as well.

## Projects

AnkhSVN consists of _a lot_ of projects in its solution.

It is assumed that **Ankh.VS.VersionThunk** and **Ankh.VS.VersionThunk.V4** originally exist to maintain compatibility with the .NET 2.0 runtime while moving the main project to .NET 4.x. This makes the architecture more complex at little gain these days, so both projects have actually been moved to 4.x at this point.

## Building

VS _should_ mostly figure out how to gather the necessary references through NuGet, with one notable exception: **Ankh.BitmapExtractor** requires you to manually fetch a few files from elsewhere, probably for licensing reasons (as also mentioned in the comment in its **Program.cs**).

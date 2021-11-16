# Contributing to MapleServer2

👍 First off, thank you for making a contribution to the project! 👍

Please take a moment to review this document in order to make the contribution process easy and effective for everyone involved.

## Rules

In the interest of fostering an open and welcoming environment, we as contributors and maintainers pledge to make participation in our project and community a pleasant experience for everyone!

- Use welcoming and inclusive language
- Be respectful of differing viewpoints and experiences
- Be accepting constructive criticism
- Focus on what is best for the community
- Show empathy towards other community members

## Pull Requests

When creating pull requests abide the following:

1. Commit messages should be clear and descriptive, small commits can be one line, but large commits should be multi-line with a title and description.
2. Pull requests should focus on as few related things as possible. Ex: one PR per bug fix.
3. Pull request titles should follow the format Feat/Fix/Cleanup/Suggestion: description.
4. Pull request descriptions should include a summarized list of changes.
5. Pull requests should be based on the most recent master repo's `master` branch.

## Coding Style

All contributions must follow the style defined in `.editorconfig`.

**On Visual Studio** &mdash; `Ctrl+K` and `Ctrl+E` to automatically format the file. Go to `Options > Text Editor > C# > Advanced` and enable full solution analysis and use `.editorconfig` compatibility mode.

**On Visual Studio Code** &mdash; `Shift+Alt+F` to automatically format the file.

Make sure to run `dotnet-format` before commiting your changes!

To install `dotnet-format` run in the terminal:

```sh
dotnet tool install -g dotnet-format --version "7.0.*" --add-source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet-tools/nuget/v3/index.json
```

## Bug Reports and Suggestions

We use [issues](https://github.com/AlanMorel/MapleServer2/issues) to track bugs and suggestions.

Before filing a new bug report perform the following:

- **Before filing an issue** &mdash; use the search function to see if a similar issue might exist already.
- **Check if the issue has been fixed** &mdash; try to reproduce it using the latest `master` branch in the repository.

Please try to be as detailed as possible in bug reports. What is your environment? What are the steps to reproduce the issue?
What is the expected outcome? All these details will help in identifying and fixing the bug.

## Final Note

All contributions will be open-source. In short, when you submit code changes, your submissions are understood to be open-source in accordance with the LICENSE.

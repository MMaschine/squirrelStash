# Repository Guidelines

## Development Rules

- Do not create `ContentPage` instances or compose page/dialog UI inline inside methods. Create a proper view in `Views` with XAML and code-behind, then show that view from helpers or view models.
- Every abstraction must have XML documentation comments that describe its contract.
- Every implementation member that satisfies a documented abstraction should use `<inheritdoc />` instead of duplicating the abstraction documentation.

## Folder Structure

Keep the current solution structure. Add new code to the existing matching project and folder instead of creating new top-level folders unless the architecture genuinely requires it.

```text
SquirrelStash/
  SquirrelStash/
  SquirrelStash.DataAccess/
  SquirrelStash.Logic/
```

Keep the MAUI app project organized by the existing folders:

```text
SquirrelStash/SquirrelStash/
  Abstractions/
  Components/
  Converters/
  Enums/
  Helpers/
  Logic/
    Factories/
  Models/
  Platforms/
    Android/
    iOS/
    MacCatalyst/
    Tizen/
    Windows/
  Properties/
  Requests/
  Resources/
    AppIcon/
    Fonts/
    Images/
    Raw/
    Splash/
    Styles/
  ViewModels/
  Views/
```

Use these placement rules:

- Put interfaces and contracts in `Abstractions`.
- Put concrete services and business/application logic in `Logic`; put factories in `Logic/Factories`.
- Put view models in `ViewModels`.
- Put XAML views and their code-behind in `Views`.
- Put reusable UI fragments/components in `Components`.
- Put value converters in `Converters`.
- Put shared enums in `Enums`.
- Put helper utilities in `Helpers`.
- Put request DTOs in `Requests` and domain/data models in `Models`.
- Put images, fonts, raw assets, and XAML styles under the matching `Resources` subfolder.
- Do not add source files under `bin` or `obj`.

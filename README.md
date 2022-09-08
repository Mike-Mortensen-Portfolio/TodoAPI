# Introduction
_Add an introduction by explaining what the idea behind the application is_

# Project Details
| Platform | GUI | Timeframe | Database Solution |
|----------|-----|-----------|-------------------|
| .NET 6 | ... | ......... | In Memory |

## Dependencies
- [Microsoft.EntityFrameworkCore.InMemory(_v6.0.8_)](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/6.0.8)
- [Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore(_v6.0.8_)](https://www.nuget.org/packages/Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore/6.0.8)

See the [WiKi](Insert_WiKi_Link_Here) for more in depth information about the project.

## Api Handlers
| Method | Handler           | Description                                          | Request Body | Response Body          |
|--------|-------------------|------------------------------------------------------|--------------|------------------------|
| GET    | `/`               | _A browsertest. Returns "Hello World"_                 | None         | `Hello, World!`        |
| GET    | `/todoitems`      | _Gets all To-do items that are not yet completed_      | None         | `Array of To-do items` |
| GET    | `/todoitems/all`  | _Gets all To-do items. Both uncompleted and completed_ | None         | `Array of To-do items` |
| GET    | `/todoitems/{id}` | _Gets the To-do item with the given Id_                | None         | `To-do item`           |
| POST   | `/todoitems`      | _Adds a new To-do item_                                | To-do item   | `No content`           |
| PUT    | `/todoitems/{id}` | _Updates the To-do item with the given Id_             | To-do item   | `No content`           |
| DELETE | `/todoItems/{id}` | _Removes the To-do item with the given Id_             | To-do item   | `No content`           |

## Web Client Specifications
- [X] ~~Index page with information about the project~~ Index page should act as the To-do item list view
- [X] A page for displaying To-do items
- [X] A details page and an edit page for To-do items
- [X] A way to delete To-do items
- [X] It should be possible to create a new To-do item from the index page
- [X] Completed To-do items should not be displayed on the index page
- [X] A ToDoService provider should be implemented
- [X] Application constants should be collected in a class

## Setup
### API
Make sure that the API has the following `User Secrets`.

```json
{
  "Auth0": {
    "Domain": "[DOMAIN]",
    "Audience": "API_IDENFITIER"
  }
}
```
The Json parameters can be obtained by creating an account on [Auth0](https://auth0.com/).
Once there you'll have to create an API application by going to 
**Dashboard** > **Applications** > **APIs** > **Create API**.

### Client
The following `User Secrets` must be present on the client,
otherwise you won't be able to authenticate and/or authorize.

```json
{
  "Auth0": {
    "Domain": "[DOMAIN]",
    "ClientId": "[CLIENT_ID]",
    "ClientSecret": "CLIENT_SECRET",
    "Audience": "API_IDENFITIER"
  }
}
```
You should already have a **Default App** on [Auth0](https://auth0.com/)
from which you can obtain the Json parameters. However, in case you do not
already have one, you can create on by going to
**Dashboard** > **Applications** > **Applications** > **Create Application**.

## Entity Relation Diagram
<details><summary>Click to show image(s)</summary>
![image](**Insert-Image**)
</details>

## Class Diagrams
<details><summary>Click to show image(s)</summary>
![image](**Insert-Image**)
</details>

# Standards
- **Versioning**
  - Version Template: _[_Major_].[_Minor_].[_Patch_]-[StateMod]_.
  - `Major:`
    - Major Changes
    - Changes to Core structure (_Like an UI Switch_)
  - `Minor:`
    - 100 _Minor_ versions = 1 _Major_ version
    - Features
    - Large Code Refactoring (_Ex. If you create a new file when refactoring_)
  - `Patch:`
    - 100 _Patch_ versions = 1 _Minor_ version
    - Hotfixs
    - Revisions
    - Minor Code Refactoring
  - `StateMod:`
    - Can either be _Dev_ or _Rel_ and defines the state of the version. _Dev_ meaning the version is on the Development branch, while _Rel_ means it's on the Release branch and therefore published.
      > Example: v0.10.3-Dev
  - When a _Major_ version is applied it resets the version count, same goes for a _Minor_ version.
      > Example: v0.55.15-Rel -> v1.0.0-Rel | v0.99.56-Dev -> v0.99.0-Dev
- **Source Control**
    - `Features` must be branched out and developed on an isolated branch and merged back into the `Developer` branch when done.
    - `Branches` must be named as follows: *[MajorVersion]/[YouInitials]/[FeatureName]*.
      > Example: v0/MSM/ExampleBranch.
- **Code**
  - `Namespaces` must be constructed as follows: _SBO.[ProjectName].[FolderName]_.
  - `Fields` must be _private_ or _protected_.
  - `Properties` must be _public_, _protected_ or _internal_.
  - `Interfaces` must have their own subfolder, which should never be included in their `namespace`.
  - `Scopes` must have explicit enclosure.

## Change Log
 - **[v0.0.0](LinkToGitLabTag)**
   - **Added**
     - _List the features added with this version_
   - **Modified**
     - _List the areas that were altered in any way in this version_
   - **Fixed**
     - _List the bugs that were fixed in this version_
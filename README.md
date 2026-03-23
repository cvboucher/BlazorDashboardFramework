# BlazorDashboardFramework

A lightweight, **drag‑and‑drop dashboard framework** for Blazor Server/WebAssembly. It provides a layout engine (rows/columns), widget registry, editable widgets, and a simple JSON‑serializable dashboard model.

---

## Features

- **Nested row/column layouts** (Bootstrap grid classes)
- **Drag‑and‑drop widget reordering** (SortableJS)
- **Edit mode** with add/edit/delete widget actions
- **Widget registry** (`WidgetTypeService`) with strongly‑typed config support
- **JSON‑serializable dashboard model** (`Dashboard`, `Row`, `Column`, `WidgetInstance`)
- **Layout presets** (`StructureService`)

---

## Requirements (client‑side libs)

Add the following client-side libraries to your app (from CDN or local):

- **Bootstrap 5** (CSS + JS)
- **Bootstrap Icons 1** (CSS)
- **SortableJS 1** (JS)

Example (wwwroot/index.html or _Host.cshtml):

```html
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css" rel="stylesheet" />
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/sortablejs@1.15.2/Sortable.min.js"></script>
```

---

## Install (project reference)

Add a reference to the library project or package it as a NuGet later.

```bash
# From your app project
# dotnet add reference ../BlazorDashboardFramework/BlazorDashboardFramework.csproj
```

---

## Register services

```csharp
// Program.cs
builder.Services.AddBlazorDashboardFramework();
```

This registers:
- `StructureService` (layout presets)
- `WidgetTypeService` (widget registry)
- `EditModeService`
- `SortableListService`
- Clock widget service

---

## Minimal usage

```razor
@page "/dashboard"

<BdfDashboard Value="_dashboard" OnDashboardSaved="Save" ReadOnly="false" />

@code {
    private Dashboard _dashboard = new();

    protected override void OnInitialized()
    {
        // Optional: start with a preset layout
        //_dashboard = new StructureService().GetDefaultLayout();
    }

    private Task Save(Dashboard dashboard)
    {
        // Persist dashboard JSON here
        return Task.CompletedTask;
    }
}
```

---

## Dashboard model

The `Dashboard` object is JSON‑serializable and contains:

- `Layout` (string) – layout name (e.g., `6-6`, `12/6-6`)
- `Title` (string)
- `Rows` → `Columns` → `WidgetInstance`

Use `dashboard.SerializeWithCamelCase()` to store JSON and `DeserializeFromCamelCase<T>()` to load.

---

## Widgets

### Register widgets

Widgets are defined in `WidgetTypeService.WidgetTypes`.

```csharp
WidgetTypeService.WidgetTypes["clock"] = new WidgetType
{
    Type = "clock",
    Title = "Clock",
    Description = "Displays the current time.",
    DisplayComponent = typeof(Widgets.Clock.ClockDisplayView),
    EditComponent = typeof(Widgets.Clock.ClockEditView),
    ConfigType = typeof(Widgets.Clock.ClockConfig)
};
```

### Widget display component

A display component receives cascading parameters:

```razor
@code {
    [CascadingParameter] public WidgetInstance WidgetInstance { get; set; } = default!;
    [CascadingParameter] public WidgetType widgetType { get; set; } = default!;
    [CascadingParameter] public BdfWidget bdfWidget { get; set; } = default!;
}
```

### Widget config

If a widget has a `ConfigType`, the edit component must expose a parameter named `Config`:

```razor
@code {
    [Parameter] public ClockConfig Config { get; set; } = default!;
}
```

---

## Edit mode

`BdfDashboard` toggles edit mode through `EditModeService`:

- **Edit mode** shows add/edit/delete controls
- **View mode** shows widget content only

---

## Layout presets

The `StructureService` provides several predefined layouts (e.g., `6-6`, `12/4-4-4`, etc.).
You can add additional presets by extending `StructureService.Structures`.

---

## Notes / Gotchas

- `SortableList` uses **SortableJS** in `wwwroot/_content/BlazorDashboardFramework/SortableList.razor.js`.
- Ensure the JS and CSS assets are available in your host app.
- If you use config types, keep them JSON‑serializable.

---

## License

MIT (see LICENSE in this repo).

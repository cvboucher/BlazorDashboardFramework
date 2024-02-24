Requires adding Bootstrap and Bootstrap-Icons to your project.

Requirements:
Add the following client side libraries to your app.
Bootstrap 5.*
Bootstrap-Icons 1.*
Sortable 1.*

Add additional Layouts to LayoutService.Layouts.
Add additional WidgetTypes to WidgetTypeService.WidgetTypes

Every ContentComponent must have the following parameter:
[Parameter] public WidgetInstance Widget { get; set; } = default!;

Every ConfigComponent must have the following parameter:
[Parameter] public ClockConfig Config { get; set; } = default!;
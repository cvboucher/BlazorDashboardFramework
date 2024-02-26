Requires adding Bootstrap and Bootstrap-Icons to your project.

Requirements:
Add the following client side libraries to your app.
Bootstrap 5.*
Bootstrap-Icons 1.*
Sortable 1.*

Add <CascadingBlazoredModal> around <Router>

Predefined layouts are available in LayoutService.Layouts.  You can define and add additional Layouts to LayoutService.Layouts.
Add additional WidgetTypes to WidgetTypeService.WidgetTypes

If a WidgetType has a ConfigType, the DisplayComponent and EditComponent must have a paramater named Config with that type.
[Parameter] public (type of ConfigType) Config { get; set; } = default!;

For each of the following EventCallback properties in WidgetType, you must have a corresponding EventCallback property in your DisplayComponent.  Set the property to true if you will be providing that EventCallback in your Display component.
SetHideWidgetEventCallback - [Parameter] public EventCallback<bool> SetHideWidget { get; set; }
SetHideHeaderEventCallback - [Parameter] public EventCallback<bool> SetHideHeader { get; set; }
ToggleHideHeaderEventCallback - [Parameter] public EventCallback ToggleHideHeader { get; set; }
SetCollapsedEventCallback - [Parameter] public EventCallback<bool> SetCollapsed { get; set; }
ToggleCollapsedEventCallback - [Parameter] public EventCallback ToggleCollapsed { get; set; }


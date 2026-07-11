const sortableInstances = new Map();

export function init(id, group, pull, put, sort, handle, filter, component, forceFallback) {
    destroy(id);

    const element = document.getElementById(id);
    if (!element) {
        return;
    }

    const sortable = new Sortable(element, {
        animation: 200,
        group: {
            name: group,
            pull: pull || true,
            put: put
        },
        filter: filter || undefined,
        sort: sort,
        forceFallback: forceFallback,
        handle: handle || undefined,
        onUpdate: (event) => {
            // Revert the DOM to match the .NET state
            event.item.remove();
            event.to.insertBefore(event.item, event.to.childNodes[event.oldIndex]);

            // Notify .NET to update its model and re-render
            component.invokeMethodAsync('OnUpdateJS', event.oldDraggableIndex, event.newDraggableIndex, event.from.id, event.to.id);
        },
        onRemove: (event) => {
            if (event.pullMode === 'clone') {
                // Remove the clone
                event.clone.remove();
            }

            event.item.remove();
            event.from.insertBefore(event.item, event.from.childNodes[event.oldIndex]);

            // Notify .NET to update its model and re-render
            component.invokeMethodAsync('OnRemoveJS', event.oldDraggableIndex, event.newDraggableIndex, event.from.id, event.to.id);
        }
    });

    sortableInstances.set(id, sortable);
}

export function destroy(id) {
    const sortable = sortableInstances.get(id);
    if (!sortable) {
        return;
    }

    sortable.destroy();
    sortableInstances.delete(id);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class Dashboard
    {
        //public string DashboardId { get; set; } = Guid.NewGuid().ToString();
        [JsonPropertyName("structure")]
        public string Layout { get; set; } = "6-6";
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("rows")]
        public List<Row> Rows { get; set; } = new();
        [JsonPropertyName("titleTemplateUrl")]
        public string? TitleTemplateUrl { get; set; }


        //public void MoveWidget(int oldIndex, int newIndex, string fromId, string toId)
        //{
        //    var fromColumn = FindColumn(fromId)
        //        ?? throw new Exception($"Unable to find from ColumnId {fromId}");
        //    var toColumn = FindColumn(toId)
        //        ?? throw new Exception($"Unable to find to ColumnId {toId}");
        //    if (fromColumn == toColumn)
        //    {
        //        var itemToMove = fromColumn.Widgets[oldIndex];
        //        fromColumn.Widgets.RemoveAt(oldIndex);
        //        if (newIndex < fromColumn.Widgets.Count)
        //            fromColumn.Widgets.Insert(newIndex, itemToMove);
        //        else
        //            fromColumn.Widgets.Add(itemToMove);
        //        fromColumn.OnWidgetsChanged();
        //    }
        //    else
        //    {
        //        var itemToMove = fromColumn.Widgets[oldIndex];
        //        toColumn.Widgets.Insert(newIndex, itemToMove);
        //        fromColumn.Widgets.Remove(fromColumn.Widgets[oldIndex]);
        //        fromColumn.OnWidgetsChanged();
        //        toColumn.OnWidgetsChanged();
        //    }
        //}

        public Column? FindFirstColumn()
        {
            foreach (var row in this.Rows)
            {
                var firstColumn = FindFirstColumn(row.Columns);
                if (firstColumn != null)
                    return firstColumn;
            }
            return null;
        }

        private Column? FindFirstColumn(List<Column> columns)
        {
            var firstColumn = columns.FirstOrDefault(x => !x.Rows.Any());
            if (firstColumn != null)
                return firstColumn;
            foreach (var column in columns)
            {
                foreach (var row in column.Rows)
                {
                    firstColumn = FindFirstColumn(row.Columns);
                    if (firstColumn != null)
                        return firstColumn;
                }
            }
            return null;
        }

        public Column? FindColumn(string columnId)
        {
            foreach (var row in this.Rows)
            {
                var column = FindColumn(row.Columns, columnId);
                if (column != null)
                    return column;
            }
            return null;
        }

        private Column? FindColumn(List<Column> columns, string columnId)
        {
            var matchingColumn = columns.FirstOrDefault(x => x.ColumnId == columnId);
            if (matchingColumn != null)
                return matchingColumn;
            foreach (var column in columns)
            {
                foreach (var row in column.Rows)
                {
                    matchingColumn = FindColumn(row.Columns, columnId);
                    if (matchingColumn != null)
                        return matchingColumn;
                }
            }
            return null;
        }

        public Queue<List<WidgetInstance>> GetAllWidgets()
        {
            var queue = new Queue<List<WidgetInstance>>();
            foreach (var row in this.Rows)
                GetAllWidgets(queue, row.Columns);
            return queue;
        }

        private Queue<List<WidgetInstance>> GetAllWidgets(Queue<List<WidgetInstance>> queue, List<Column> columns)
        {
            foreach (var column in columns)
            {
                if (column.Rows.Any())
                    foreach (var row in column.Rows)
                        GetAllWidgets(queue, row.Columns);
                else
                    queue.Enqueue(column.Widgets);
            }
            return queue;
        }

        public void PopulateWidgets(Queue<List<WidgetInstance>> queue)
        {
            if (!this.Rows.Any())
                return;
            while (queue.Any())
                PopulateWidgets(queue, this.Rows);
        }

        private void PopulateWidgets(Queue<List<WidgetInstance>> queue, List<Row> rows)
        {
            foreach (var row in rows)
                PopulateWidgets(queue, row.Columns);
        }

        private void PopulateWidgets(Queue<List<WidgetInstance>> queue, List<Column> columns)
        {
            foreach (var column in columns)
                if (column.Rows.Any())
                    PopulateWidgets(queue, column.Rows);
                else if (queue.Any())
                    column.Widgets.AddRange(queue.Dequeue());
        }

        public void RegenerateIds()
        {
            foreach (var row in this.Rows)
                RegenerateIds(row.Columns);
        }

        public void RegenerateIds(List<Column> columns)
        {
            foreach (var column in columns)
            {
                column.ColumnId = Guid.NewGuid().ToString();
                foreach (var widget in  column.Widgets)
                    widget.WidgetInstanceId = Guid.NewGuid().ToString();
                foreach (var row in column.Rows)
                    RegenerateIds(row.Columns);
            }
        }

    }
}

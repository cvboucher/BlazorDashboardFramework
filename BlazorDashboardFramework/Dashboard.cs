﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorDashboardFramework
{
    public class Dashboard
    {
        public Guid DashboardId { get; set; } = Guid.NewGuid();
        public string Layout { get; set; } = "1";
        public bool ReadOnly { get; set; }
        public string? Title { get; set; }
        public List<Row> Rows { get; set; } = new();


        public void MoveWidget(int oldIndex, int newIndex, string fromId, string toId)
        {
            var fromColumn = FindColumn(fromId)
                ?? throw new Exception($"Unable to find from ColumnId {fromId}");
            var toColumn = FindColumn(toId)
                ?? throw new Exception($"Unable to find to ColumnId {toId}");
            if (fromColumn == toColumn)
            {
                var itemToMove = fromColumn.Widgets[oldIndex];
                fromColumn.Widgets.RemoveAt(oldIndex);
                if (newIndex < fromColumn.Widgets.Count)
                    fromColumn.Widgets.Insert(newIndex, itemToMove);
                else
                    fromColumn.Widgets.Add(itemToMove);
                fromColumn.OnWidgetsChanged();
            }
            else
            {
                var itemToMove = fromColumn.Widgets[oldIndex];
                toColumn.Widgets.Insert(newIndex, itemToMove);
                fromColumn.Widgets.Remove(fromColumn.Widgets[oldIndex]);
                fromColumn.OnWidgetsChanged();
                toColumn.OnWidgetsChanged();
            }
        }

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


    }
}

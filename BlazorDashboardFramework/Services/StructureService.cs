namespace BlazorDashboardFramework.Services
{
    /// <summary>
    /// Dashboard layouts ("structures"). Names and style classes match the original
    /// angular-dashboard-framework app (adf-structures-base.js and adf-structures-pieceCounter.js)
    /// so the "structure" value stored in old dashboards resolves to the same layout.
    /// </summary>
    public class StructureService
    {
        public const string DefaultStructureName = "3-9 (12/4-4-4)";

        public Dictionary<string, Dashboard> Structures { get; } = new();

        public StructureService()
        {
            // angular-dashboard-framework base structures
            Add("6-6", R(C("col-md-6"), C("col-md-6")));
            Add("4-8", R(C("col-md-4"), C("col-md-8")));
            Add("12/4-4-4", R(C("col-md-12")), R(C("col-md-4"), C("col-md-4"), C("col-md-4")));
            Add("12/6-6", R(C("col-md-12")), R(C("col-md-6"), C("col-md-6")));
            Add("12/6-6/12", R(C("col-md-12")), R(C("col-md-6"), C("col-md-6")), R(C("col-md-12")));
            Add("3-9 (12/6-6)", R(C("col-md-3"), C("col-md-9", R(C("col-md-12")), R(C("col-md-6"), C("col-md-6")))));

            // PieceCounter structures
            Add("12/3-3-3-3", R(C("col-md-12")), R(C("col-md-3"), C("col-md-3"), C("col-md-3"), C("col-md-3")));
            Add("3-9 (12/4-4-4)", R(C("col-md-3"), C("col-md-9", R(C("col-md-12")), R(C("col-md-4"), C("col-md-4"), C("col-md-4")))));
            Add("12/3-6-3/12", R(C("col-md-12")), R(C("col-md-3"), C("col-md-6"), C("col-md-3")), R(C("col-md-12")));
            Add("12/6-6/4-4-4/3-3-3-3/2-2-2-2-2-2",
                R(C("col-md-12")),
                R(C("col-md-6"), C("col-md-6")),
                R(C("col-md-4"), C("col-md-4"), C("col-md-4")),
                R(C("col-md-3"), C("col-md-3"), C("col-md-3"), C("col-md-3")),
                R(C("col-md-2"), C("col-md-2"), C("col-md-2"), C("col-md-2"), C("col-md-2"), C("col-md-2")));
            Add("2-10 (12/6-6/4-4-4/3-3-3-3)",
                R(C("col-sm-2"), C("col-sm-10",
                    R(C("col-sm-12")),
                    R(C("col-sm-6"), C("col-sm-6")),
                    R(C("col-sm-4"), C("col-sm-4"), C("col-sm-4")),
                    R(C("col-sm-3"), C("col-sm-3"), C("col-sm-3"), C("col-sm-3")))));
            Add("3-9 (12/3-3-3-3)", R(C("col-md-3"), C("col-md-9", R(C("col-md-12")), R(C("col-md-3"), C("col-md-3"), C("col-md-3"), C("col-md-3")))));
        }

        /// <summary>A fresh copy of the default layout for new dashboards.</summary>
        public Dashboard GetDefaultLayout()
        {
            return GetLayout(DefaultStructureName)
                ?? throw new Exception("Default layout not found.");
        }

        public Dashboard? GetFirstOrDefault()
        {
            var name = Structures.Keys.FirstOrDefault();
            return name == null ? null : GetLayout(name);
        }

        /// <summary>
        /// A fresh copy of the named layout (case-insensitive), with <see cref="Dashboard.Layout"/> set
        /// to the structure name and new column ids. Null if the name is unknown.
        /// </summary>
        public Dashboard? GetLayout(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            var match = Structures.FirstOrDefault(x => name.Equals(x.Key, StringComparison.OrdinalIgnoreCase));
            if (match.Value == null)
                return null;
            var layout = match.Value.DeepCopy()
                ?? throw new Exception($"Unable to copy layout {name}.");
            layout.Layout = match.Key;
            layout.Title = match.Key;
            layout.RegenerateIds();
            return layout;
        }

        public bool Contains(string? name)
            => !string.IsNullOrWhiteSpace(name) && Structures.Keys.Any(x => name.Equals(x, StringComparison.OrdinalIgnoreCase));

        private void Add(string name, params Row[] rows)
        {
            Structures.Add(name, new Dashboard() { Title = name, Layout = name, Rows = rows.ToList() });
        }

        private static Row R(params Column[] columns) => new() { Columns = columns.ToList() };

        private static Column C(string styleClass, params Row[] rows) => new() { StyleClass = styleClass, Rows = rows.ToList() };
    }
}

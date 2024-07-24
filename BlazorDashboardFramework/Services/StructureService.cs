using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorDashboardFramework.Services
{
    public class StructureService
    {

        public Dictionary<string, Dashboard> Structures { get; } = new();

        public StructureService()
        {
            Structures.Add(SixSixStructure.Title, SixSixStructure);
            Structures.Add(FourEightStructure.Title, FourEightStructure);
            Structures.Add(TwelveFourFourFourStructure.Title, TwelveFourFourFourStructure);
            Structures.Add(TwelveSixSixStructure.Title, TwelveSixSixStructure);
            Structures.Add(TwelveSixSixTwelveStructure.Title, TwelveSixSixTwelveStructure);
            Structures.Add(ThreeNineTwelveSixsixStructure.Title, ThreeNineTwelveSixsixStructure);
            Structures.Add(TwelveThreeThreeThreeThreeStructure.Title, TwelveThreeThreeThreeThreeStructure);
            Structures.Add(ThreeNineTwelveFourFourFourStructure.Title, ThreeNineTwelveFourFourFourStructure);
            Structures.Add(TwelveThreeSixThreeTwelveStructure.Title, TwelveThreeSixThreeTwelveStructure);
        }

        public Dashboard GetDefaultLayout()
        {
            var layout = ThreeNineTwelveFourFourFourStructure?.DeepCopy() ??
                throw new Exception("Default layout not found.");
            //layout.Title = null;
            return layout;
        }

        public Dashboard? GetFirstOrDefault()
        {
            var layout = Structures.Values.FirstOrDefault()?.DeepCopy();
            //if (layout != null)
            //    layout.Title = null;
            return layout;
        }

        public Dashboard? GetLayout(string title)
        {
            var layout = Structures.Values
                .FirstOrDefault(x => title.Equals(x.Title, StringComparison.OrdinalIgnoreCase))?
                .DeepCopy();
            //if (layout != null)
            //    layout.Title = null;
            return layout;
        }

        private Dashboard SixSixStructure = new Dashboard()
        {
            Title = "6-6",
            Rows = new List<Row>() {
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-6" },
                        new Column() { StyleClass = "col-md-6" }
                    }
                }
            }
        };

        private Dashboard FourEightStructure = new Dashboard()
        {
            Title = "4-8",
            Rows = new List<Row>() {
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-4" },
                        new Column() { StyleClass = "col-md-8" }
                    }
                }
            }
        };

        private Dashboard TwelveFourFourFourStructure = new Dashboard()
        {
            Title = "12/4-4-4",
            Rows = new List<Row>() {
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-12" }
                    }
                },
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-4" },
                        new Column() { StyleClass = "col-md-4" },
                        new Column() { StyleClass = "col-md-4" }
                    }
                }
            }
        };

        private Dashboard TwelveSixSixStructure = new Dashboard()
        {
            Title = "12/6-6",
            Rows = new List<Row>() {
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-12" }
                    }
                },
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-6" },
                        new Column() { StyleClass = "col-md-6" }
                    }
                }
            }
        };

        private Dashboard TwelveSixSixTwelveStructure = new Dashboard()
        {
            Title = "12/6-6/12",
            Rows = new List<Row>() {
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-12" }
                    }
                },
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-6" },
                        new Column() { StyleClass = "col-md-6" }
                    }
                },
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-12" }
                    }
                }
            }
        };

        private Dashboard ThreeNineTwelveSixsixStructure = new Dashboard()
        {
            Title = "3-9 (12/6-6)",
            Rows = new List<Row>() {
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-3" },
                        new Column()
                        {
                            StyleClass = "col-md-9",
                            Rows = new List<Row>
                            {
                                new Row()
                                {
                                    Columns = new List<Column>()
                                    {
                                        new Column() { StyleClass = "col-md-12"}
                                    }
                                },
                                new Row()
                                {
                                    Columns = new List<Column>()
                                    {
                                        new Column() { StyleClass = "col-md-6"},
                                        new Column() { StyleClass = "col-md-6"}
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        private Dashboard TwelveThreeThreeThreeThreeStructure = new Dashboard()
        {
            Title = "12/3-3-3-3",
            Rows = new List<Row>() {
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-12" }
                    }
                },
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-3" },
                        new Column() { StyleClass = "col-md-3" },
                        new Column() { StyleClass = "col-md-3" },
                        new Column() { StyleClass = "col-md-3" }
                    }
                }
            }
        };

        private Dashboard ThreeNineTwelveFourFourFourStructure = new Dashboard()
        {
            Title = "3-9 (12/4-4-4)",
            Rows = new List<Row>() {
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-3" },
                        new Column()
                        {
                            StyleClass = "col-md-9",
                            Rows = new List<Row>
                            {
                                new Row()
                                {
                                    Columns = new List<Column>()
                                    {
                                        new Column() { StyleClass = "col-md-12"}
                                    }
                                },
                                new Row()
                                {
                                    Columns = new List<Column>()
                                    {
                                        new Column() { StyleClass = "col-md-4"},
                                        new Column() { StyleClass = "col-md-4"},
                                        new Column() { StyleClass = "col-md-4"}
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        private Dashboard TwelveThreeSixThreeTwelveStructure = new Dashboard()
        {
            Title = "12/3-6-3/12",
            Rows = new List<Row>() {
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-12" }
                    }
                },
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-3" },
                        new Column() { StyleClass = "col-md-6" },
                        new Column() { StyleClass = "col-md-3" }
                    }
                },
                new Row()
                {
                    Columns = new List<Column>()
                    {
                        new Column() { StyleClass = "col-md-12" }
                    }
                }
            }
        };
    }
}

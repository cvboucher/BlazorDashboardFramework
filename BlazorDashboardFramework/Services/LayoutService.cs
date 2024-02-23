using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorDashboardFramework.Services
{
    public class LayoutService
    {

        public Dictionary<string, Dashboard> Layouts { get; } = new();

        public LayoutService()
        {
            Layouts.Add(SixSixStructure.Title, SixSixStructure);
            Layouts.Add(FourEightStructure.Title, FourEightStructure);
            Layouts.Add(TwelveFourFourFourStructure.Title, TwelveFourFourFourStructure);
            Layouts.Add(TwelveSixSixStructure.Title, TwelveSixSixStructure);
            Layouts.Add(TwelveSixSixTwelveStructure.Title, TwelveSixSixTwelveStructure);
            Layouts.Add(ThreeNineTwelveSixsixStructure.Title, ThreeNineTwelveSixsixStructure);
            Layouts.Add(TwelveThreeThreeThreeThreeStructure.Title, TwelveThreeThreeThreeThreeStructure);
            Layouts.Add(ThreeNineTwelveFourFourFourStructure.Title, ThreeNineTwelveFourFourFourStructure);
            Layouts.Add(TwelveThreeSixThreeTwelveStructure.Title, TwelveThreeSixThreeTwelveStructure);
        }

        public Dashboard GetDefaultLayout()
        {
            return ThreeNineTwelveFourFourFourStructure?.DeepCopy() ??
                throw new Exception("Default layout not found.");
        }

        public Dashboard? GetFirstOrDefault()
        {
            return Layouts.Values.FirstOrDefault()?.DeepCopy();
        }

        public Dashboard? GetLayout(string title)
        {
            return Layouts.Values
                .FirstOrDefault(x => title.Equals(x.Title, StringComparison.OrdinalIgnoreCase))?
                .DeepCopy();
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

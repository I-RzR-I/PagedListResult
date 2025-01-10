// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.FilterTests
//  Author           : RzR
//  Created On       : 2023-11-05 15:27
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-07 12:15
// ***********************************************************************
//  <copyright file="AsFiltrableQueryTests.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using FilterTests.Data;
using FilterTests.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PagedListResult.Common.Extensions.Filters;
using PagedListResult.DataModels.Enums;
using PagedListResult.DataModels.Models.Request;

#endregion

namespace FilterTests.Filters.QueryFiltrable
{
    [TestClass]
    public class AsFiltrableQueryTests
    {
        private IQueryable<TestItemDto> _fakeItems;

        [TestInitialize]
        public void Initialize() => _fakeItems = FakeItemData.InitInfo();

        [TestMethod]
        public void AsFilterable_Simple_With_Main_Filter_Should_Be_2_Test()
        {
            //Act
            var filtered = _fakeItems.AsFilterable(new List<DataFilter>
            {
                new DataFilter
                {
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "price", 
                        Values = new List<string> { "3" }, 
                        Condition = FilterType.Equals
                    }
                }
            });

            //Assert
            filtered.Count().Should().Be(2);
        }

        [TestMethod]
        public void AsFilterable_Simple_With_Main_Filter_Condition_And_Should_Be_3_Test()
        {
            //Act
            var filtered = _fakeItems.AsFilterable(
                new List<DataFilter>
                {
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue
                        {
                            PropertyName = "price", 
                            Values = new List<string> { "3" }, 
                            Condition = FilterType.Equals
                        }
                    },
                    new DataFilter
                    {
                        FilterValue = new DataFilterValue
                        {
                            PropertyName = "price", 
                            Values = new List<string> { "2" }, 
                            Condition = FilterType.Equals
                        }
                    }
                }, FilterConditionType.Or);

            //Assert
            filtered.Count().Should().Be(3);
        }

        [TestMethod]
        public void AsFilterable_Depend_1_And_Should_1_Test()
        {
            //Act
            var filtered = _fakeItems.AsFilterable(new List<DataFilter>
            {
                new DataFilter
                {
                    FilterApplyOrder = 1,
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "price", 
                        Values = new List<string> { "3" }, 
                        Condition = FilterType.Equals
                    },
                    Dependencies = new List<DataFilterDependence>
                    {
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.And,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "Count", 
                                Values = new List<string> { "101" }, 
                                Condition = FilterType.Equals
                            }
                        }
                    }
                }
            });

            //Assert
            filtered.Count().Should().Be(1);
        }

        [TestMethod]
        public void AsFilterable_Depend_1_Or_Should_2_Test()
        {
            //Act
            var filtered = _fakeItems.AsFilterable(new List<DataFilter>
            {
                new DataFilter
                {
                    FilterApplyOrder = 1,
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "price", 
                        Values = new List<string> { "3" }, 
                        Condition = FilterType.Equals
                    },
                    Dependencies = new List<DataFilterDependence>
                    {
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.Or,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "Count",
                                Values = new List<string> { "101" },
                                Condition = FilterType.Equals
                            }
                        }
                    }
                }
            });

            //Assert
            filtered.Count().Should().Be(2);
        }

        [TestMethod]
        public void AsFilterable_Depend_Count_2_Or_Should_2_Test()
        {
            //Act
            var filtered = _fakeItems.AsFilterable(new List<DataFilter>
            {
                new DataFilter
                {
                    FilterApplyOrder = 1,
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "name", 
                        Values = new List<string> { "test" }, 
                        Condition = FilterType.Contains
                    },
                    Dependencies = new List<DataFilterDependence>
                    {
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.Or,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "Count", 
                                Values = new List<string> { "5" }, 
                                Condition = FilterType.Equals
                            }
                        },
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.Or,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "Count", 
                                Values = new List<string> { "10" },
                                Condition = FilterType.Equals
                            }
                        }
                    }
                }
            });

            //Assert
            filtered.Count().Should().Be(2);
        }

        [TestMethod]
        public void AsFilterable_Name_Contains_test_Depend_Count_5_Or_Count_10_And_IsActive_True_Should_2_Test()
        {
            //Act
            var filtered = _fakeItems.AsFilterable(new List<DataFilter>
            {
                new DataFilter
                {
                    FilterApplyOrder = 1,
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "name", 
                        Values = new List<string> { "test" }, 
                        Condition = FilterType.Contains
                    },
                    Dependencies = new List<DataFilterDependence>
                    {
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.Or,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "Count",
                                Values = new List<string> { "5" },
                                Condition = FilterType.Equals
                            }
                        },
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.Or,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "Count",
                                Values = new List<string> { "10" },
                                Condition = FilterType.Equals
                            }
                        },
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.And,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "isActive",
                                Values = new List<string> { "true" }, 
                                Condition = FilterType.Equals
                            }
                        }
                    }
                }
            });

            //Assert
            filtered.Count().Should().Be(2);
        }

        [TestMethod]
        public void AsFilterable_Should_3_Test()
        {
            //Act
            var filtered = _fakeItems.AsFilterable(new List<DataFilter>
            {
                new DataFilter { 
                    FilterApplyOrder = 0, 
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "id", 
                        Values = new List<string> { "1", "3", "2" },
                        Condition = FilterType.IsIn
                    } },
                new DataFilter
                {
                    FilterApplyOrder = 1,
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "count", 
                        Values = new List<string> { "2" },
                        CompareValue = "109", Condition = FilterType.Between
                    },
                    Dependencies = new List<DataFilterDependence>
                    {
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.Or, 
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "isBlocked", 
                                Condition = FilterType.IsNull
                            }
                        },
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.Or,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "isActive", 
                                Values = new List<string> { "true" }, 
                                Condition = FilterType.Equals
                            }
                        },
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.And,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "minQuantity",
                                Values = new List<string> { "0" },
                                Condition = FilterType.Equals
                            }
                        }
                    }
                }
            });

            //Assert
            filtered.Count().Should().Be(3);
        }

        [TestMethod]
        public void AsFilterable_Should_3x_Test()
        {
            //Act
            var filtered = _fakeItems.AsFilterable(new List<DataFilter>
            {
                new DataFilter { 
                    FilterApplyOrder = 0, 
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "id", 
                        Values = new List<string> { "1", "3", "2", "0" }, 
                        Condition = FilterType.IsIn
                    } },
                new DataFilter
                {
                    FilterApplyOrder = 1,
                    FilterValue = new DataFilterValue
                    {
                        PropertyName = "count", 
                        Values = new List<string> { "7" }, 
                        CompareValue = "109", 
                        Condition = FilterType.Between
                    },
                    Dependencies = new List<DataFilterDependence>
                    {
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.Or, 
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "isBlocked", 
                                Condition = FilterType.IsNull
                            }
                        },
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.Or,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "isActive", 
                                Values = new List<string> { "true" }, 
                                Condition = FilterType.Equals
                            }
                        },
                        new DataFilterDependence
                        {
                            ParentFilterLinkType = FilterConditionType.And,
                            FilterValue = new DataFilterValue
                            {
                                PropertyName = "minQuantity",
                                Values = new List<string> { "0" }, 
                                Condition = FilterType.Equals
                            }
                        }
                    }
                }
            });

            //Assert
            filtered.Count().Should().Be(4);
        }
    }
}
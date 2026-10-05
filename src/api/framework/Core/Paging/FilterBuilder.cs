using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Framework.Core.Paging;

//public class FilterBuilder
//{
//    private readonly FilterGroup _rootGroup;

//    private FilterBuilder(FilterLogic logic)
//    {
//        _rootGroup = new FilterGroup
//        {
//            Logic = logic,
//            Filters = new List<Filter>(),
//            FilterGroups = new List<FilterGroup>()
//        };
//    }

//    // Entry points to start building
//    public static FilterBuilder And => new FilterBuilder(FilterLogic.And);
//    public static FilterBuilder Or => new FilterBuilder(FilterLogic.Or);

//    // Adds a direct primitive filter rule to the current group
//    public FilterBuilder Where(string field, FilterOperator op, object value)
//    {
//        _rootGroup.Filters.Add(new Filter
//        {
//            Field = field,
//            Operator = op,
//            Value = value
//        });
//        return this;
//    }

//    // Allows nesting a group inside the current root group
//    public FilterBuilder Group(FilterBuilder nestedBuilder)
//    {
//        if (_rootGroup.FilterGroups == null)
//        {
//            _rootGroup.FilterGroups = new List<FilterGroup>();
//        }

//        _rootGroup.FilterGroups.Add(nestedBuilder.Build());
//        return this;
//    }

//    // Finalizes the structural chain
//    public FilterGroup Build() => _rootGroup;

//    /*                Example Using Filter Builder                  */

//    //// Build the complex dynamic criteria elegantly
//    //var ruleEngineFilter = FilterBuilder.And
//    //    .Where("Category", FilterOperator.Equals, "Electronics")
//    //    .Group(FilterBuilder.Or
//    //        .Where("Rating", FilterOperator.GreaterThanOrEqual, 4.5)
//    //        .Where("Price", FilterOperator.LessThanOrEqual, 50)
//    //    )
//    //    .Build();

//    //// Assign it straight to your request object
//    //var request = new SearchProductsRequest
//    //{
//    //    PageNumber = 1,
//    //    PageSize = 20,
//    //    Filter = ruleEngineFilter
//    //};

//    /*                Example Using JSON Payload Structure                */

//    //
//    //  This is the raw JSON structure that your frontend clients (or API test tools like Postman)
//    //  will send to the backend. It maps directly to the nested FilterGroup configuration.
//    // 
//    //  {
//    //    "pageNumber": 1,
//    //    "pageSize": 20,
//    //    "orderBy": ["name asc"],
//    //    "filter": {
//    //      "logic": "And",
//    //      "filters": [
//    //        {
//    //          "field": "Category",
//    //          "operator": "Equals",
//    //          "value": "Electronics"
//    //        }
//    //      ],
//    //      "filterGroups": [
//    //        {
//    //          "logic": "Or",
//    //          "filters": [
//    //            {
//    //              "field": "Rating",
//    //              "operator": "GreaterThanOrEqual",
//    //              "value": 4.5
//    //            },
//    //            {
//    //      "field": "Price",
//    //              "operator": "LessThanOrEqual",
//    //              "value": 50
//    //            }
//    //          ]
//    //        }
//    //      ]
//    //    }
//    //  }


//}

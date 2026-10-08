
using FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode.Examples;
public class ProductModelExample
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsInStock { get; set; }

    public ProductModelExample()
    {
        // Define top-level group (AND)
        var rootGroup = new FilterGroup
        {
            GroupOperator = LogicalGroupOperator.And,
            Rules = new List<FilterRule>
    {
        new FilterRule { PropertyName = "IsInStock", Operator = "Equals", Value = true }
    },
            Groups = new List<FilterGroup>
    {
        // Inner nested subgroup (OR)
        new FilterGroup
        {
            GroupOperator = LogicalGroupOperator.Or,
            Groups = new List<FilterGroup>
            {
                // Subgroup 1: Cheap Electronics
                new FilterGroup
                {
                    GroupOperator = LogicalGroupOperator.And,
                    Rules = new List<FilterRule>
                    {
                        new FilterRule { PropertyName = "Category", Operator = "Equals", Value = "Electronics" },
                        new FilterRule { PropertyName = "Price", Operator = "<=", Value = 500 }
                    }
                },
                // Subgroup 2: Cheap Accessories
                new FilterGroup
                {
                    GroupOperator = LogicalGroupOperator.And,
                    Rules = new List<FilterRule>
                    {
                        new FilterRule { PropertyName = "Category", Operator = "Equals", Value = "Accessories" },
                        new FilterRule { PropertyName = "Price", Operator = "<=", Value = 50 }
                    }
                }
            }
        }
    }
        };

        // Compile to Expression
        var compiledFilter = TreeNodeFilterBuilder<ProductModelExample>.BuildGroupFilter(rootGroup);

        // Apply to tree
        TreeNode<ProductModelExample> rootNode = GetSampleProductTree();
        TreeNodeUtility<ProductModelExample>.Filter(rootNode, compiledFilter);
    }

    private TreeNode<ProductModelExample> GetSampleProductTree()
    {
        // TODO: Implement the method to return a sample product tree
        throw new NotImplementedException();
    }

    public string ExportToJson(FilterGroup  rootGroup)
    {
        string jsonOutput = TreeNodeFilterBuilder<ProductModelExample>.ExportToJson(rootGroup);
        Console.WriteLine(jsonOutput);
        return jsonOutput;
    }

    public TreeNode<ProductModelExample> ImportFromJson(TreeNode<ProductModelExample> rootNode)
    {
        string savedJsonFilter = GetSavedJsonFilterFromDatabaseOrFile();

        // Deserializes and compiles expression tree directly
        var filterExpression = TreeNodeFilterBuilder<ProductModelExample>.ImportFromJson(savedJsonFilter);

        // Filter the TreeNode
        TreeNodeUtility<ProductModelExample>.Filter(rootNode, filterExpression);
        return rootNode;
    }

    private string GetSavedJsonFilterFromDatabaseOrFile()
    {
        throw new NotImplementedException();
    }
}

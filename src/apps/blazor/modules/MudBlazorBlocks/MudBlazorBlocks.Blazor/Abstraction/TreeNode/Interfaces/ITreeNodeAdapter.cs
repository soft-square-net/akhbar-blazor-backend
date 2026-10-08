using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.Blazor.Modules.MudBlazorBlocks.Blazor.Abstraction.TreeNode.Interfaces;

public interface ITreeNodeAdapter<TNode> where TNode : class, ITreeNodeAdapter<TNode>
{
    string Id { get; }
    TNode? Parent { get; set; }
    string? ParentId { get; set; }
    List<TNode> Children { get; }
    bool IsExpanded { get; set; }
    bool IsVisible { get; set; }

    bool MatchesQuery(string query);
    void AddChildData(TNode childNode);
    void RemoveChildData(TNode childNode);
}

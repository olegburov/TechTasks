﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
  public class UndirectedGraph
  {
    private Dictionary<string,Vertex> vertexes;

    public UndirectedGraph()
    {
      this.vertexes = new Dictionary<string, Vertex>();
    }

    public void AddVertex(Vertex item)
    {
      Assert.ArgumentNotNull(item, nameof(item));

      if (this.vertexes.ContainsKey(item.Name))
      {
        throw new ArgumentException($"The vertex with the key '{item.Name}' already exists in the graph.");
      }

      this.vertexes.Add(item.Name, item);
    }

    public void RemoveVertex(Vertex item)
    {
      Assert.ArgumentNotNull(item, nameof(item));

      if (this.Exist(item))
      {
        // Remove vertex's edges first, then remote vertex itself. 
        foreach (var edge in item.Edges)
        {
          this.RemoveEdge(item, edge.Vertex);
        }

        this.vertexes.Remove(item.Name);
      }
    }

    public void AddEdge(Vertex start, Vertex end, int weight)
    {
      Assert.ArgumentNotNull(start, nameof(start));
      Assert.ArgumentNotNull(end, nameof(end));

      if (this.Exist(start) && this.Exist(end))
      {
        // Two vertexes must be unique.
        if (start == end)
        {
          throw new ArgumentException($"An edge cannot be added between the single vertex '{start.Name}'.");
        }

        start.AddEdge(new Edge(weight, end));
        end.AddEdge(new Edge(weight, start));
      }
    }

    public void RemoveEdge(Vertex start, Vertex end)
    {
      Assert.ArgumentNotNull(start, nameof(start));
      Assert.ArgumentNotNull(end, nameof(end));

      if (this.Exist(start) && this.Exist(end))
      {
        var forwardEdge = start.Edges.Single(s => s.Vertex.Name.Equals(end.Name));
        start.RemoveEdge(forwardEdge);

        var backwardEdge = end.Edges.Single(s => s.Vertex.Name.Equals(start.Name));
        end.RemoveEdge(backwardEdge);
      }
    }

    public bool Exist(Vertex item)
    {
      Assert.ArgumentNotNull(item, nameof(item));

      if (!this.vertexes.ContainsKey(item.Name))
      {
        throw new ArgumentException($"The graph does not contains the vertex '{item.Name}'.");
      }

      return true;
    }

    public int FindMaxWeightedPath(Vertex start, Vertex end)
    {
      Assert.ArgumentNotNull(start, nameof(start));
      Assert.ArgumentNotNull(end, nameof(end));

      return this.FindMaxWeightedPathImpl(start, end, new List<Vertex>());
    }

    #region Private Method

    private int FindMaxWeightedPathImpl(Vertex current, Vertex end, List<Vertex> visited)
    {
      if (current == end)
      {
        return 0;
      }

      var maxWeight = 0;
      visited.Add(current);

      foreach (var edge in current.Edges)
      {
        if (!visited.Contains(edge.Vertex))
        {
          var currentWeight = edge.Weight + this.FindMaxWeightedPathImpl(edge.Vertex, end, visited);

          if (maxWeight < currentWeight)
          {
            maxWeight = currentWeight;
          }
        }
      }

      visited.Remove(current);

      return maxWeight;
    }

    #endregion
  }
}

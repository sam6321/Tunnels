using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TileNode
{
    public TileNode()
    {

    }

    public TileNode(int xIn, int yIn)
    {
        x = xIn;
        y = yIn;
    }

    public bool Equals(TileNode other)
    {
        return x == other.x && y == other.y;
    }

    public int x;
    public int y;
    public int totalCost;
    public TileNode parent;
}

class TileOracle
{
    private TileGenerator tileGenerator;

    public TileOracle(TileGenerator tileGeneratorIn)
    {
        tileGenerator = tileGeneratorIn;
    }

    public List<TileNode> GetAccessibleTiles(TileNode node)
    {
        List<TileNode> nodes = new List<TileNode>();
        if(BelowMap(node))
            return nodes;

        bool canDig = tileGenerator.GetTile(node.x, node.y + 1) != null;
        int xMax = tileGenerator.GetSize().x - 1;
        for(int x = node.x - 1; x <= node.x + 1; x++)
        {
            for(int y = node.y; y <= node.y + 1; y++)
            {
                if((x == node.x && y == node.y) || (x == node.x - 1 && y == node.y + 1) || (x == node.x + 1 && y == node.y + 1) || x > xMax || x < 0 || (!canDig && x != node.x))
                    continue;

                var thing = tileGenerator.GetTile(x, y);
                Tile tile = thing ? thing.GetComponent<Tile>() : null;

                if(tile == null || (canDig && !tile.GetInvulnerable()))
                {
                    nodes.Insert(0, new TileNode(x, y));
                }
            }
        }
        return nodes;
    }

    public int Cost(TileNode node)
    {
        var gameObject = tileGenerator.GetTile(node.x, node.y);
        if(gameObject == null)
            return 2;

        var tile = gameObject.GetComponent<Tile>();
        var resource = gameObject.GetComponent<TileResourceOnBreak>();
        var resourceBonus = resource == null ? 1 : -10 * (int)(resource.GetResourceType() + 2);
        var tileCost = tile == null ? 3 : tile.GetHealth();
        return resourceBonus + tileCost;
    }

    public bool BelowMap(TileNode node)
    {
        return node.y > tileGenerator.GetSize().y;
    }
}

public class AiPathPlanner
{
    private TileOracle tileOracle;

    public AiPathPlanner(TileGenerator tileGeneratorIn)
    {
        tileOracle = new TileOracle(tileGeneratorIn);
    }

    public List<TileNode> GetAiPath(TileNode start)
    {
        start.parent = start;
        var openList = new List<TileNode>();
        var closedList = new List<TileNode>();

        openList.Add(start);

        while(openList.Count > 0)
        {
            var lowest = openList.Min(l => l.totalCost);
            TileNode current = openList.First(l => l.totalCost == lowest);

            closedList.Add(current);
            openList.Remove(current);

            if(closedList.FirstOrDefault(l => tileOracle.BelowMap(l)) != null){
                break;
            }

            var accessibleTile = tileOracle.GetAccessibleTiles(current);

            foreach(var adjacentTile in accessibleTile)
            {
                if(closedList.FirstOrDefault(l => l.Equals(adjacentTile)) != null)
                    continue;

                var newCost = current.totalCost + tileOracle.Cost(adjacentTile);

                var inOpenList = openList.FirstOrDefault(l => l.Equals(adjacentTile)) != null;

                if(!inOpenList || newCost < adjacentTile.totalCost)
                {
                    adjacentTile.totalCost = newCost;
                    adjacentTile.parent = current;
                }

                if(!inOpenList)
                    openList.Insert(0, adjacentTile);
            }
        }

        List<TileNode> path = new List<TileNode>();
        var last = closedList.FirstOrDefault(l => tileOracle.BelowMap(l));

        if(last != null)
        {
            path.Add(last);
            while(path.First().parent != start)
            {
                path.Insert(0, path.First().parent);
            }
        }

        return path;
    }
}


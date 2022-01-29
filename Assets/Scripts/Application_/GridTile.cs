using System;
using UnityEngine;

/*
 *
 * Base tile to create the Grid
 */
namespace Application_
{
    [Serializable]
    public class GridTile
    {
        public Vector2 tuplePosition;
        public TileEntity tileEntity;
        public GridTile cameFromNode;
        public float FValue;
        public float GCost; //Walking cost from the Start position to Destination
        public float HValue; //Heuristic cost to reach Destination
    
        public GridTile()
        {
            tuplePosition = new Vector2(-1, -1);
            FValue = 0;
            GCost = 0;
            HValue = 0;
        }


        public float GetFCost()
        {
            FValue = GCost + HValue;
            return FValue;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic.Algorithms;
using Ventura.Util;

namespace Ventura.Test
{


    public class TestVisibilityAlgorithm : MonoBehaviour
    {
        private static Color visibleColor = Color.green;
        private static Color notVisibleColor = Color.blue;
        private static Color startColor = Color.red;
        private static Color endColor = Color.yellow;

        public GameObject terrainTileTemplate;
        public Transform terrainLayer;

        private GameObject[,] _mapTiles;

        private static int MAP_WIDTH = 12;
        private static int MAP_HEIGHT = 20;


        private void Awake()
        {
            _mapTiles = new GameObject[MAP_WIDTH, MAP_HEIGHT];
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    var newMapTile = Instantiate(terrainTileTemplate, new Vector3(x, y), Quaternion.identity);
                    newMapTile.GetComponent<SpriteRenderer>().color = notVisibleColor;
                    newMapTile.transform.SetParent(terrainLayer);
                    _mapTiles[x, y] = newMapTile;
                }
            }
        }

        public void OnButtonClicked()
        {
            var startPos = new Vector2Int(Random.Range(0, MAP_WIDTH), Random.Range(0, MAP_HEIGHT));
            var endPos = new Vector2Int(Random.Range(0, MAP_WIDTH), Random.Range(0, MAP_HEIGHT));

            //var startPos = new Vector2Int(0, 1);
            //var endPos = new Vector2Int(1, 6);


            var blockingTiles = new bool[MAP_WIDTH, MAP_HEIGHT];
            var visibleLine = Visibility.ComputeLine(startPos, endPos);
            DebugUtils.Log("Computed line: ");
            foreach (var pos in visibleLine)
                DebugUtils.Log($"{pos}");

            updateView(visibleLine, startPos, endPos);
        }


        private void updateView(List<Vector2Int> visibleLine, Vector2Int startPos, Vector2Int endPos)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    _mapTiles[x, y].GetComponent<SpriteRenderer>().color = notVisibleColor;
                }
            }

            foreach (var pos in visibleLine)
            {
                _mapTiles[pos.x, pos.y].GetComponent<SpriteRenderer>().color = visibleColor;
            }

            _mapTiles[startPos.x, startPos.y].GetComponent<SpriteRenderer>().color = startColor;
            _mapTiles[endPos.x, endPos.y].GetComponent<SpriteRenderer>().color = endColor;
        }

    }
}

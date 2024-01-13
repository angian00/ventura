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
        private static Color wallColor = Color.grey;

        public GameObject terrainTileTemplate;
        public Transform terrainLayer;

        private GameObject[,] _mapTiles;

        private static int MAP_WIDTH = 42;
        private static int MAP_HEIGHT = 30;
        private static int VISIBILITY_RADIUS = 12;


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
            //runBresenham();
            runShadowcasting();
        }

        private void runShadowcasting()
        {

            var blockingTiles = new bool[MAP_WIDTH, MAP_HEIGHT];
            for (int x = 0; x < MAP_WIDTH; x++)
                for (int y = 0; y < MAP_HEIGHT; y++)
                    blockingTiles[x, y] = false;

            const int nObstacles = 40;
            for (int iObstacle = 0; iObstacle < nObstacles; iObstacle++)
            {
                var x = Random.Range(0, MAP_WIDTH);
                var y = Random.Range(0, MAP_HEIGHT);
                blockingTiles[x, y] = true;
            }

            int startX, startY;
            do
            {
                startX = Random.Range(0, MAP_WIDTH);
                startY = Random.Range(0, MAP_HEIGHT);
            } while (blockingTiles[startX, startY]);



            var startPos = new Vector2Int(startX, startY);

            var visibilityAlgo = new Visibility(blockingTiles);
            var visibleTiles = visibilityAlgo.ComputeVisibility(startPos, VISIBILITY_RADIUS);

            updateView(blockingTiles, visibleTiles, startPos);
        }


        private void runBresenham()
        {
            var startPos = new Vector2Int(Random.Range(0, MAP_WIDTH), Random.Range(0, MAP_HEIGHT));
            var endPos = new Vector2Int(Random.Range(0, MAP_WIDTH), Random.Range(0, MAP_HEIGHT));

            var visibleLine = Bresenham.ComputeLine(startPos, endPos);
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


        private void updateView(bool[,] blockingTiles, bool[,] visibleTiles, Vector2Int startPos)
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    Color tileColor;

                    if (blockingTiles[x, y])
                        tileColor = wallColor;
                    else if (visibleTiles[x, y])
                        tileColor = visibleColor;
                    else
                        tileColor = notVisibleColor;
                    _mapTiles[x, y].GetComponent<SpriteRenderer>().color = tileColor;
                }
            }

            _mapTiles[startPos.x, startPos.y].GetComponent<SpriteRenderer>().color = startColor;
        }

    }
}

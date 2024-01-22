using System.Collections.Generic;
using UnityEngine;
using Ventura.Util;
using Color = UnityEngine.Color;

namespace Ventura.Test
{
    class MeshNode
    {
        public Vector2 pos;
        public float height;

        public MeshNode(Vector2 pos)
        {
            this.pos = pos;
        }
    }

    //class MeshEdge
    //{
    //    public MeshNode node1;
    //    public MeshNode node2;

    //    public MeshEdge(MeshNode node1, MeshNode node2)
    //    {
    //        this.node1 = node1;
    //        this.node2 = node2;
    //    }
    //}

    class MeshTriangle
    {
        public MeshNode node1;
        public MeshNode node2;
        public MeshNode node3;
        public float height;
        public bool isLand;

        public MeshTriangle(MeshNode node1, MeshNode node2, MeshNode node3)
        {
            this.node1 = node1;
            this.node2 = node2;
            this.node3 = node3;
        }
    }


    public class TestWorldGenerator2 : MonoBehaviour
    {
        [Header("Mesh Settings")]
        public float worldWidth = 4.0f;
        public float worldHeight = 3.0f;
        public int meshWidth;
        public int meshHeight;
        public float meshNoiseFactor;

        [Header("Heightmap Settings")]
        public int heightmapNumOctaves = 5;
        public float heightmapLacunarity = 2.0f;
        public float heightmapPersistence = 0.5f;
        public AnimationCurve heightRedistribution = AnimationCurve.Linear(0, 0, 1, 1);
        //public float seaLevel = 0.5f;
        private float seaLevel = 0.1f; //heightRedistribution is supposed to flatten all sea to ~ 0.0

        [Header("Drawing Settings")]
        public Material drawMaterial;
        public Color waterColor = Color.blue;
        public Color landLowColor = Color.green;
        public Color landHighColor = Color.white;
        public Transform mapContainer;


        private MeshNode[,] meshNodes;
        //private List<MeshEdge> meshEdges;
        private List<MeshTriangle> meshTriangles;


        void Start()
        {
            GenerateMap();
        }


        public void GenerateMap()
        {
            Debug.Log("TestWorldGenerator2.Start");
            var t0 = Time.realtimeSinceStartup;

            initMesh();
            var tMesh = Time.realtimeSinceStartup;
            DebugUtils.Log($"tMesh duration : {(tMesh - t0):f2} seconds");

            initLand();
            var tLand = Time.realtimeSinceStartup;
            DebugUtils.Log($"tLand duration : {(tLand - tMesh):f2} seconds");

            drawMap();
            var tDraw = Time.realtimeSinceStartup;
            DebugUtils.Log($"tDraw duration : {(tDraw - tLand):f2} seconds");

        }


        private void initMesh()
        {
            meshNodes = new MeshNode[meshWidth, meshHeight];
            //meshEdges = new();
            meshTriangles = new();

            for (int iMesh = 0; iMesh < meshWidth; iMesh++)
            {
                for (int jMesh = 0; jMesh < meshHeight; jMesh++)
                {
                    //TODO: displace nodes pulling from other nodes, using random node weights
                    var xPos = worldWidth * ((float)iMesh) / meshWidth + (Random.value - .5f) * meshNoiseFactor;
                    var yPos = worldHeight * ((float)jMesh) / meshHeight + (Random.value - .5f) * meshNoiseFactor;

                    meshNodes[iMesh, jMesh] = new MeshNode(new Vector2(xPos, yPos));

                    //if (iMesh > 0)
                    //    addEdge(iMesh, jMesh, iMesh - 1, jMesh);

                    //if (jMesh > 0)
                    //    addEdge(iMesh, jMesh, iMesh, jMesh - 1);

                    if (iMesh > 0 && jMesh > 0)
                    {
                        var n1 = meshNodes[iMesh - 1, jMesh - 1];
                        var n2 = meshNodes[iMesh - 1, jMesh];
                        var n3 = meshNodes[iMesh, jMesh];
                        var n4 = meshNodes[iMesh, jMesh - 1];

                        var flipTriangle = RandomUtils.RandomBool();
                        if (flipTriangle)
                        {
                            meshTriangles.Add(new MeshTriangle(n1, n2, n4));
                            meshTriangles.Add(new MeshTriangle(n2, n3, n4));
                        }
                        else
                        {
                            meshTriangles.Add(new MeshTriangle(n1, n2, n3));
                            meshTriangles.Add(new MeshTriangle(n3, n4, n1));
                        }
                    }
                }
            }
        }

        //private MeshEdge addEdge(int iFrom, int jFrom, int iTo, int jTo)
        //{
        //    var startNode = meshNodes[iFrom, jFrom];
        //    var endNode = meshNodes[iTo, jTo];

        //    var meshEdge = new MeshEdge(startNode, endNode);
        //    meshEdges.Add(meshEdge);

        //    //drawLine(startNode.pos, endNode.pos);
        //    //drawDebugLine(meshNodes[iFrom, jFrom], meshNodes[iTo, jTo]);
        //    return meshEdge;
        //}



        private void initLand()
        {
            var minNoise = float.MaxValue;
            var maxNoise = float.MinValue;

            //randomizes seed at every invocation of initLand
            var xOffset = Random.value;
            var yOffset = Random.value;

            for (int iMesh = 0; iMesh < meshWidth; iMesh++)
            {
                for (int jMesh = 0; jMesh < meshHeight; jMesh++)
                {
                    var currNode = meshNodes[iMesh, jMesh];

                    var noiseVal = 0.0f;
                    var amplitude = 1.0f;
                    var frequency = 1.0f;
                    for (int i = 0; i < heightmapNumOctaves; i++)
                    {
                        noiseVal += amplitude * Mathf.PerlinNoise(
                             frequency * currNode.pos.x + xOffset,
                             frequency * currNode.pos.y + yOffset);

                        frequency *= heightmapLacunarity;
                        amplitude *= heightmapPersistence;
                    }

                    currNode.height = noiseVal;

                    if (noiseVal < minNoise)
                        minNoise = noiseVal;
                    if (noiseVal > maxNoise)
                        maxNoise = noiseVal;
                }
            }

            //renormalize values to range from 0.0 to 1.0, then apply remapping function
            for (int iMesh = 0; iMesh < meshWidth; iMesh++)
            {
                for (int jMesh = 0; jMesh < meshHeight; jMesh++)
                {
                    var meshNode = meshNodes[iMesh, jMesh];
                    var normalizedHeight = DataUtils.RescaleValue(meshNode.height, minNoise, maxNoise);
                    normalizedHeight = heightRedistribution.Evaluate(normalizedHeight);
                    meshNode.height = normalizedHeight;
                }
            }

            //update triangles
            foreach (var meshTriangle in meshTriangles)
            {
                meshTriangle.height = (meshTriangle.node1.height + meshTriangle.node2.height + meshTriangle.node3.height) / 3;
                meshTriangle.isLand = (meshTriangle.height > seaLevel);
            }
        }


        private void drawMap()
        {
            UnityUtils.RemoveAllChildren(mapContainer);

            foreach (var meshTriangle in meshTriangles)
            {
                drawTriangle(meshTriangle);
            }
        }


        private void drawTriangle(MeshTriangle meshTriangle)
        {
            GameObject triangleObj = new GameObject();
            triangleObj.name = "MeshTriangle";

            var renderer = triangleObj.AddComponent<MeshRenderer>();
            renderer.material = drawMaterial;
            renderer.material.color = meshTriangle.isLand ? Color.Lerp(landLowColor, landHighColor, meshTriangle.height) : waterColor;

            var unityMesh = triangleObj.AddComponent<MeshFilter>().mesh;
            unityMesh.Clear();
            unityMesh.vertices = new Vector3[] { meshTriangle.node1.pos, meshTriangle.node2.pos, meshTriangle.node3.pos };
            unityMesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 0.25f), new Vector2(0.25f, 0.25f) };
            unityMesh.triangles = new int[] { 0, 1, 2 };

            triangleObj.transform.SetParent(mapContainer);
        }


        private void drawLine(Vector2 start, Vector2 end)
        {
            GameObject lineObj = new GameObject();
            lineObj.name = "MeshLine";
            lineObj.transform.position = start;

            var lr = lineObj.AddComponent<LineRenderer>();
            lr.material = drawMaterial;
            lr.startWidth = 0.002f;
            lr.endWidth = 0.002f;

            lr.useWorldSpace = true;
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        private static void drawDebugLine(Vector2 start, Vector2 end)
        {
            Debug.Log($"drawLine from {start} to {end}");

            const float lineDurationSeconds = 30.0f;
            Debug.DrawLine(start, end, Color.white, lineDurationSeconds);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Actions;
using Ventura.Util;

namespace Ventura.GameLogic.Components
{
public abstract class AI
{
        protected Orchestrator _orch;
        protected Actor _parent;

        protected AI(Orchestrator orch, Actor parent)
        {
            this._orch = orch;
            this._parent = parent;
        }

        /** 
         * Compute and return a path to the target position.
         * If there is no valid path then returns an empty list.
         */
        protected List<Vector2Int> GetPathTo(int destX, int destY) {
            var targetMap = _orch.CurrMap;
            //var walkables = new bool[,]();

            for (var x=0; x < targetMap.Width; x++) {
			    //walkables.push([]);
			    for (var y = 0; y < targetMap.Height; y++) {
                    //walkables[x].push(map.tiles[x][y].walkable);
                }
		    }

		    foreach (var e in targetMap.Entities)
            {
                if (e.IsBlocking && !(e == _parent) && !(e.x == destX && e.y == destY))
                    //walkables[e.x][e.y] = false;
                    ;
            }

            //TODO: use Unity pathfinding algorithm

            //        let passableDelegate = function(x: number, y: number): boolean
            //        {
            //            if (x < 0 || x >= map.width || y < 0 || y >= map.height)
            //                return false


            //            return walkables[x][y]

            //        }

            var outputPath = new List<Vector2Int>();

            //        let outputCallback = function(x: number, y: number): void
            //{
            //    outputPath.push([x, y])
            //  }

            //let dijkstra = new Path.Dijkstra(destX, destY, passableCallback, null)
            //  dijkstra.compute(this.parent.x, this.parent.y, outputCallback)

            //  //remove starting position from path
            //  outputPath.shift()


            return outputPath;
        }

        public abstract GameAction? ChooseAction();
        //public abstract AI Clone(Actor newParent);
        //public abstract ToObject()

        //static fromObject(obj: any): AI
        //{
        //    if (obj == "EnemyAI")
        //        return new EnemyAI(null, null)

        //        else if (obj == "PlayerAI")
        //        return new PlayerAI(null, null)

        //        else
        //    return null

        //}
    }


    public class EnemyAI : AI
    {
        private List<Vector2Int> _path = new();

        public EnemyAI(Orchestrator orch, Actor parent) : base(orch, parent) { }


        public override GameAction? ChooseAction()
        {
            DebugUtils.Log("EnemyAI.chooseAction");

            var target = _orch.Player;
            var dx = target.x - _parent.y;
            var dy = target.y - _parent.y;

            var distance = Math.Max(Math.Abs(dx), Math.Abs(dy));


            if (_orch.CurrMap.Visible[_parent.x, _parent.y])
            {
                //if monster is visible to player, 
                //then player is visible to monster
                if (distance <= 1)
                    return new MeleeAction(_orch, _parent, dx, dy);


                _path = GetPathTo(target.x, target.y);
            }

            if (_path.Count > 0)
            {
                var dest = _path[0];
                _path.RemoveAt(0);

                DebugUtils.Log("EnemyAI chose MovementAction");
                return new MovementAction(_orch, _parent, dest.x - _parent.x, dest.y - _parent.y);
            }

            return new WaitAction(_orch, _parent);
        }
    }


    public class PlayerAI: AI
    {
        public PlayerAI(Orchestrator orch, Actor parent) : base(orch, parent) { }

        public override GameAction? ChooseAction() {
            return _orch.DequeuePlayerAction();
        }

    }
}

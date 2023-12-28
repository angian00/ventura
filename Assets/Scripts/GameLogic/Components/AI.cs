using System;
using System.Collections.Generic;
using UnityEngine;
using Ventura.GameLogic;
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
                if (e.IsBlocking && !(e == _parent) && !(e.X == destX && e.Y == destY))
                    //walkables[e.x][e.y] = false;
                    ;
            }

            //TODO: use Unity pathfinding algo

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

        public abstract Action? ChooseAction();
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


        public override Action? ChooseAction()
        {
            Messages.Log("EnemyAI.chooseAction");

            var target = _orch.Player;
            var dx = target.X - _parent.Y;
            var dy = target.Y - _parent.Y;

            var distance = Math.Max(Math.Abs(dx), Math.Abs(dy));


            if (_orch.CurrMap.Visible[_parent.X, _parent.Y])
            {
                //if monster is visible to player, 
                //then player is visible to monster
                if (distance <= 1)
                    return new MeleeAction(_orch, _parent, dx, dy);


                _path = GetPathTo(target.X, target.Y);
            }

            if (_path.Count > 0)
            {
                var dest = _path[0];
                _path.RemoveAt(0);

                Messages.Log("EnemyAI chose MovementAction");
                return new MovementAction(_orch, _parent, dest.x - _parent.X, dest.y - _parent.Y);
            }

            return new WaitAction(_orch, _parent);
        }
    }


    public class PlayerAI: AI
    {
        public PlayerAI(Orchestrator orch, Actor parent) : base(orch, parent) { }

        public override Action? ChooseAction() {
            return _orch.DequeuePlayerAction();
        }

    }
}

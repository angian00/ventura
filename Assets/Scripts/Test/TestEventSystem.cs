using UnityEngine;
using Ventura.GameLogic;
using Ventura.GameLogic.Components;
using Ventura.Unity.Events;
using Ventura.Util;

namespace Ventura.Unity.Behaviours
{
    public class TestEventSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            EventManager.ContainerUpdateEvent.AddListener(dumpContainer);
        }

        private void OnDisable()
        {
            EventManager.ContainerUpdateEvent.RemoveListener(dumpContainer);

        }

        private void dumpContainer(Container c)
        {
            DebugUtils.Log($"dumpContainer triggered");
            DebugUtils.Log($"it's a: {c.GetType()}");
            if (c is Inventory)
            {
                var inv = (Inventory)c;
                DebugUtils.Log($"parent: {inv.Parent.Name}");
            }

            DebugUtils.Log($"content:");
            foreach (var gi in c.Items)
                gi.Dump();
        }
    }
}
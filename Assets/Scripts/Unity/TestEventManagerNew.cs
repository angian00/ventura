using UnityEngine;

namespace Ventura.Unity.Events
{
    public class TestEventManagerNew : MonoBehaviour
    {

        void Start()
        {
            //EventManager.Publish(this, new TextNotification("hello from TestEventManagerNew", TextNotification.Severity.Critical));
            EventManager.Publish(new TextNotification("hello from TestEventManagerNew", TextNotification.Severity.Critical));
        }
    }
}

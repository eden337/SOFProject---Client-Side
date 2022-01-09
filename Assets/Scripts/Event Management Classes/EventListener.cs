
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityGameObjectEvent: UnityEvent<TraveledNode>{};

public class EventListener : MonoBehaviour
{
    public Event gEvent;
    public UnityGameObjectEvent response = new UnityGameObjectEvent();
    

    private void OnEnable() {
        gEvent.Register(this);
    }

    private void OnDisable() {
        gEvent.Unregister(this);
    }

    public void OnEventOccurs(TraveledNode node){
        response.Invoke(node);
    }
}

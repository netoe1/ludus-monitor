using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

interface IMonitorMouseData
{

    Vector3 GetMousePosition();
    void IsPointerNotMoving();
}
interface IMonitorTime
{

}

public class Monitor :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerMoveHandler,
    IMonitorMouseData,
    IEventSystemHandler
{

    private Vector3 lastMousePosition;
    void Start()
    {
        Debug.Log("[monitor-status]: Script started on gameobject '" + this.gameObject.name + "' id:" + this.gameObject.GetInstanceID());
    }

    // Update is called once per frame
    void Update()
    {
        IsPointerNotMoving();
        lastMousePosition = GetMousePosition();
    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        string side = "undefined";

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            side = "left";
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            side = "right";
        }

        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            side = "middle";
        }


        Debug.Log("[monitor-mouse-click]: The " + side + " has been pressed.");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Vector3 pos  = GetMousePosition();
        Debug.Log("[monitor-mouse-move]: Current position:(" + pos.x + "," + pos.y + "," + pos.z + ")");
    }
    public void IsPointerNotMoving()
    {
        if (lastMousePosition == GetMousePosition())
        {
            Debug.Log("[monitor-mouse-not-move]: The pointer is in the same position.");
        }
    }

}

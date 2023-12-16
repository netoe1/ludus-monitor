using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


interface IMouseData
{
    Vector3 GetMousePosition();
}

interface IConfigCollider
{
    void ConfigBoxCollider();
}

public class Monitor :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerMoveHandler,
    IMouseData,
    IConfigCollider
{
    void Start()
    {
        ConfigBoxCollider();
        Debug.Log("[monitor-status]: Script started on gameobject '" + this.gameObject.name + "' id:" + this.gameObject.GetInstanceID());
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ConfigBoxCollider()
    {
        try
        {

            if (!this.gameObject.GetComponent<BoxCollider2D>())
            {
                throw new UnityException("[monitor-error-internals]: The current gameobject doesn't have a box collider 2d.");
            }

            if (!this.gameObject.GetComponent<BoxCollider2D>().isActiveAndEnabled)
            {
                throw new UnityException("[monitor-error-internals]: The current gameobject aren't active. Please, active it in Unity Inspector.");
            }

            this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            this.gameObject.GetComponent<BoxCollider2D>().autoTiling = true;
            this.gameObject.GetComponent<BoxCollider2D>().size = this.gameObject.GetComponent<RectTransform>().rect.size;
        }
        catch(UnityException err)
        {
            throw err;
        }
    }

    public Vector3 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        string side = "undefined";

        if (Input.GetMouseButtonDown(0))
        {
            side = "left";
        }
        else if (Input.GetMouseButtonDown(1))
        {
            side = "right";
        }
        else if (Input.GetMouseButtonDown(2))
        {
            side = "middle";
        }

        Debug.Log("[monitor-mouse-click]: The " + side + " button has been pressed.");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Vector3 pos = GetMousePosition();
        Debug.Log("[monitor-mouse-move]: The pointer has changed direction.");
        Debug.Log("[monitor-mouse-move]: Current position:(" + pos.x + "," + pos.y + "," + pos.z + ")");
    }

    

}

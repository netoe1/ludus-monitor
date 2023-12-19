using JetBrains.Annotations;
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;


interface IMonitorMouseData
{
    //  Mouse
    Vector3 GetMousePosition();
    void IsPointerNotMoving();

    // Clicking:
    void ClickOnRightButton();
    void ClickOnLeftButton();
    void ClickOnMiddleButton();

    void InitClickHandler();

}
interface IMonitorTime
{
    void StartMonitoringTime();
    TimeSpan GetCurrentTime();
    void EndMonitoringTime();
}

public class Monitor :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerMoveHandler,
    IMonitorMouseData,
    IMonitorTime
{

    //  Attributes declaration


    //  IMonitorMouseData
    private Vector3 lastMousePosition;
    private int rightButtonClick;
    private int leftButtonClick;
    private int middleButtonClick;

    //  IMonitorTime
    private Stopwatch timeCounter = new Stopwatch();

    // Log Classes... I have to implement later...
    //LogController logController = new LogController();

    void Awake()
    {
        this.StartMonitoringTime();
        this.InitClickHandler();
        UnityEngine.Debug.Log("[monitor-status]: Script started on gameobject '" + this.gameObject.name + "' id:" + this.gameObject.GetInstanceID());
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
            this.ClickOnLeftButton();
            side = "left";
        }

        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            this.ClickOnMiddleButton();
            side = "middle";
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            this.ClickOnRightButton();
            side = "right";
        }

        UnityEngine.Debug.Log("[monitor-mouse-click]: The " + side + " has been pressed.");
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        Vector3 pos = GetMousePosition();
        UnityEngine.Debug.Log("[monitor-mouse-move]: Current position:(" + pos.x + "," + pos.y + "," + pos.z + ")");
    }
    public void IsPointerNotMoving()
    {
        if (lastMousePosition == GetMousePosition())
        {
            UnityEngine.Debug.Log("[monitor-mouse-not-move]: The pointer is in the same position.");
        }
    }
    public void StartMonitoringTime()
    {
        this.timeCounter.Start();
    }
    public void EndMonitoringTime()
    {
        this.timeCounter.Stop();
    }
    public TimeSpan GetCurrentTime()
    {
        return timeCounter.Elapsed;
    }
    public void ClickOnRightButton()
    {
        this.rightButtonClick++;
    }
    public void ClickOnLeftButton()
    {
        this.leftButtonClick++;
    }
    public void ClickOnMiddleButton()
    {
        this.middleButtonClick++;
    }
    public void InitClickHandler()
    {
        this.rightButtonClick = this.leftButtonClick = this.middleButtonClick = 0;
    }

}

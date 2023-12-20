using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

interface IMonitorMouseData
{
    Vector3 GetMousePosition(); // Returns the mouse position.
    void IsPointerNotMoving();  // Verify if pointer is changing coordinates.
    void ClickOnRightButton();  // Add a click in a counter that's monitoring right click's quantities.
    void ClickOnLeftButton();   // Add a click in a counter that's monitoring left click's quantities.
    void ClickOnMiddleButton(); // Add a click in a counter that's monitoring middle click's quantities.
    void InitClickHandler();    // Init clickHandler, reseting values to zero, to avoid log errors.
    //  To implement later:
    //void ShowAllCounterInfo(); 
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
    
    //  IMonitorMouseData
    private Vector3 lastMousePosition;
    private int rightButtonClick;
    private int leftButtonClick;
    private int middleButtonClick;

    //  IMonitorTime
    private Stopwatch timeCounter = new Stopwatch(); // This object allow to count time, to use in logs.

    // Log Classes... I have to implement later...
    //LogController logController = new LogController();

    void Awake()
    {
        this.StartMonitoringTime(); // This object is used to count the execution time of the script.
        this.InitClickHandler(); 
        UnityEngine.Debug.Log("[monitor-status-awake]: Script started on gameobject '" + this.gameObject.name + "' id:" + this.gameObject.GetInstanceID()); // Log to show user that script is on!
    }

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
        try
        {        
            Vector3 pos = GetMousePosition();
            UnityEngine.Debug.Log("[monitor-mouse-move]: Current position:(" + pos.x + "," + pos.y + "," + pos.z + ")");
        }
        catch (UnityException err)
        {
            throw err;
        }
    }
    public void IsPointerNotMoving()
    {
        if (lastMousePosition == GetMousePosition())
        {       
            UnityEngine.Debug.Log("[monitor-mouse-not-move]: The pointer is in the same position.");
        }

        lastMousePosition = GetMousePosition();
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

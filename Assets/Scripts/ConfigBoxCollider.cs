using UnityEngine;
using UnityEngine.EventSystems;


public class ConfigBoxCollider :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler
{

    private BoxCollider2D __box_collider;
    private RectTransform __rectTransform;



    private void Awake()
    {
        try
        {
            __box_collider = this.gameObject.GetComponent<BoxCollider2D>();
            __rectTransform = this.gameObject.GetComponent<RectTransform>();


            if (__box_collider == null)
            {
                throw new UnityException("[boxCollider2D-config-awake-err]: Error on load Collider.");
            }

            if (__rectTransform == null)
            {
                throw new UnityException("[boxCollider2D-config-awake-err]: Error on load RectTransform.");
            }

            __box_collider.autoTiling = true;
            __box_collider.size = new Vector2(__rectTransform.rect.size.x, __rectTransform.rect.size.y);
        }
        catch (UnityException err)
        {
            throw err;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        string side = "undefined";

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            side = "left";
        }

        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            side = "middle";
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            side = "right";
        }

        UnityEngine.Debug.Log("[monitor-mouse-click-gameobj]: The " + side + " has been pressed in a gameobject.");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UnityEngine.Debug.Log("[monitor-mouse-hover-gameobj]: The mouse is on top of a gameObject now. ID:" + this.gameObject.GetInstanceID() + " Name: " + this.gameObject.name);
    }
}

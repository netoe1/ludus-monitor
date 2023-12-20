using System.ComponentModel;
using UnityEngine;

public class ConfigBoxCollider : MonoBehaviour
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

            if(__rectTransform == null)
            {
                throw new UnityException("[boxCollider2D-config-awake-err]: Error on load RectTransform.");
            }

            __box_collider.autoTiling = true;
            __box_collider.size = __rectTransform.rect.size;
        }
        catch(UnityException err)
        {
            throw err;
        }
    }

    private void OnMouseEnter()
    {
        UnityEngine.Debug.Log("[monitor-mouse-hover-gameobj]: The mouse is on top of a gameObject now. ID:" + this.gameObject.GetInstanceID() + " Name: " + this.gameObject.name);   
    }
}

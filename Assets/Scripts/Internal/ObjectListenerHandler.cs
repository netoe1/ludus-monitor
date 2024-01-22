using UnityEngine;
using UnityEngine.EventSystems;


public class ObjectListenerHandler :
    MonoBehaviour,          // Classe da Unity
    IPointerClickHandler,   // Interface feita monitorar cliques, é próprio da Unity.
    IPointerEnterHandler,   // Interface feita para detectar entradas do mouse dentro do gameObject atrelado.
    IDragHandler,           // Interface para detectar a escuta do movimento de arrastar.
    IEndDragHandler,        // Interface para detectar o final do movimento de arrastar.
    IBeginDragHandler       // Interface para detectar o inicio do movimento de arrastar.
{
    private BoxCollider2D __box_collider; // Variável que detecta o box-collider2d do objeto que está atrelado.
    private RectTransform __rectTransform; // Variável para armazenar o valor do rect transform.

    private void Awake() // O método awake() foi utilizado para iniciar junto com o script, independente da execução do programa; inicia junto com o script.
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
    public void OnPointerClick(PointerEventData eventData)      // Interface herdada:   IPointerClickHandler
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

        UnityEngine.Debug.Log("[+LUDUS-mouse-click]: The " + side + " has been pressed in a gameobject.");
    }  
    public void OnPointerEnter(PointerEventData eventData)      // Interface herdada:   IPointerEnterHandler
    {
        try
        {
            UnityEngine.Debug.Log("[+LUDUS-mouse-hover]: O mouse está em cima de um gameObject com ID:" + this.gameObject.GetInstanceID() + " e nome: " + this.gameObject.name);
        }
        catch(UnityException err)
        {
            throw err;
        }
    }
    public void OnDrag(PointerEventData eventData)              // Interface herdada:   IDragHandler
    {
        try
        {
            UnityEngine.Debug.Log("[+LUDUS-mouse-drag]: O mouse está arrastando um objeto.");
        }
        catch (UnityException err)
        {
            throw err;
        }
    }
    public void OnEndDrag(PointerEventData eventData)           // Interface herdada:   IEndDragHandler 
    {
        try
        {
            UnityEngine.Debug.Log("[+LUDUS-mouse-EndDrag]: O mouse parou de arrastar um objeto.");
        }
        catch (UnityException err)
        {
            throw err;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)         // Interface herdada:   IBeginDragHandler 
    {
        try
        {
            UnityEngine.Debug.Log("[+LUDUS-mouse-beginDrag]: O mouse está começando a arrastar um objeto.");
        }
        catch (UnityException err)
        {
            throw err;
        }
    }
}

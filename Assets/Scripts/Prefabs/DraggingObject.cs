using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Este script foi feito para configurar o Objeto que ser� usado no drag.
/// Recomendo o uso do prefab j� feito, mas se voc� quiser modificar este script para o seu pr�prio uso, fique � vontade.
/// Feito pela equipe do Projeto +LUDUS.
/// Ely Torres Neto, 2024.
/// </summary>
/// 

/*
    IDragHandler,           // Interface para detectar a escuta do movimento de arrastar.
    IEndDragHandler,        // Interface para detectar o final do movimento de arrastar.
    IBeginDragHandler       // Interface para detectar o inicio do movimento de arrastar.
 */


interface IDraggingObjectStatus         // Interface criada para pegar os dados do objeto a ser arrastado.
{
    bool IsGlued();                     // Verificar se o item est� colado.
    void SetGluedStatus(bool value);    // Modificar o estado da vari�vel isGlued()
    string GetTagFromDraggingObject();  // Pegar a tag do item do objeto a ser arrastado.
}

public class DraggingObject :
    MonoBehaviour,
    IDraggingObjectStatus,  // Interface para gerar objeto.
    IDragHandler,           // Interface para detectar a escuta do movimento de arrastar.
    IEndDragHandler,        // Interface para detectar o final do movimento de arrastar.
    IBeginDragHandler       // Interface para detectar o inicio do movimento de arrastar.
{
    
    private bool isGlued = false;

    [SerializeField] private Canvas canvas;     // Receber os dados do Canvas, � necess�rio atrel�-lo no script.
    private RectTransform m_RectTransform;      // Recebe os dados do gameObject que foi atrelado. (this.gameObject).
    private Vector3 m_RectTransform_pos0;       // Recebe os dados da posi��o inicial do gameobject iniciado. 
    private CanvasGroup m_CanvasGroup;          // Recebe os dados do canvas group atrelado ao prefab. (this.gameObject).

    // Vari�veis constantes
    const float DEF_ALPHA_AMOUNT = 1.0f;        // Definindo a quantidade padr�o do alpha do rgba()
    const float DEF_ALPHA_DRAGGING = 0.7f;      // Definindo a quantidade padr�o do alpha enquanto ele est� arrastando.

  
    private void Awake()
    {
        // Configurando rectTransform:
        m_RectTransform = GetComponent<RectTransform>();    // Recebendo os dados do rect
        m_RectTransform_pos0 = m_RectTransform.position;    // Armazenando os dados da posi��o inicial a partir do rect recebido.
        
        // Configurando m_CanvasGroup:
        m_CanvasGroup = GetComponent<CanvasGroup>();        // Recebendo o canvas group, para poder manipular alguns fatores de design.
        m_CanvasGroup.alpha = DEF_ALPHA_AMOUNT;

        isGlued = false;

    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        m_CanvasGroup.alpha = DEF_ALPHA_DRAGGING;   // Definindo o alpha do dragging.
        m_CanvasGroup.blocksRaycasts = false;       // Desligando a colis�o
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;   // Mudando a posi��o
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    { 
        m_CanvasGroup.alpha = DEF_ALPHA_AMOUNT;                     // Definindo o alpha do dragging padr�o de volta.
        m_CanvasGroup.blocksRaycasts = true;                        // Ligando a posi��o novamente
        m_RectTransform.anchoredPosition = m_RectTransform_pos0;    // Voltando a posi��o padr�o
    }

    bool IDraggingObjectStatus.IsGlued()
    {
        return this.isGlued;
    }

    string IDraggingObjectStatus.GetTagFromDraggingObject()
    {
        return this.gameObject.tag;
    }

    void IDraggingObjectStatus.SetGluedStatus(bool value)
    {
        try
        {
            this.isGlued = value;  
        }
        catch(UnityException err)
        {
            throw err;
        }
    }
}

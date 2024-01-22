using JetBrains.Annotations;
using System;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


// Script feito por Ely Torres Neto para projeto +LUDUS.

// Sobre as vari�veis constantes com o prefixo PREFIX:
//      Elas ser�o usadas como textos padronizados para os logs, isso facilita na hora de tratar erros e dar display nas
//      mensagens, pois � um label padr�o, ent�o, se mudarmos uma vez, atualiza no c�digo inteiro.

interface IMonitorMouseData     // Essa interface � respons�vel por obter os dados do mouse.
{
    Vector3 GetMousePosition(); //  M�todo Get() que retorna a posi��o do mouse em coordenadas.
    void IsPointerNotMoving();  //  Verifica se o pointeiro est� se mexendo.
    void ClickOnRightButton();  //  Adiciona um clique na v�riavel de controle de nro de cliques para o bot�o direito.
    void ClickOnLeftButton();   //  Adiciona um clique na v�riavel de controle de nro de cliques para o bot�o esquerdo.
    void ClickOnMiddleButton(); //  Adiciona um clique na v�riavel de controle de nro de cliques para o bot�o do meio.
    void InitClickHandler();    //  Inicia o handler de click, ou seja, "inicia a interface", zerando todos os valores que ser�o utilizados.
}
interface IMonitorTime          //  Essa interface � respons�vel por gerenciar os contadores de tempo que ser�o utilizados.
{
    // OBSERVA��O IMPORTANTE: Essas primeiras fun��es, modificam apenas o TimeSpan e StopWatch do pr�prio Monitor, o qual j� � instanciado por padr�o.
    void StartMonitoringTime(); //  Inicia o contador de tempo�.
    TimeSpan GetCurrentTime();  // Obt�m data e hora padr�o. Similar ao Date.Now() do Javascript.
    void EndMonitoringTime();   // Termina o tempo de monitoramento.
}

public class Monitor :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerMoveHandler,
    IMonitorMouseData,
    IMonitorTime
{


    // Prefixos que ser�o utilizados de string para os logs.
    const string PREFIX_LUDUS_WARN = "[+LUDUS-WARNING]:";
    const string PREFIX_LUDUS_SUCCESS = "[+LUDUS-SUCCESS]:";
    const string PREFIX_LUDUS_ERROR = "[+LUDUS-ERROR]:";
   
    //  IMonitorMouseData
    private Vector3 lastMousePosition; // Buffer que guarda a ultima posi��o do mouse.
    // Contadores que armazenam as quantidades de clique em cada bot�o do mouse.
    private int QTD_rightButtonClick = 0;
    private int QTD_leftButtonClick = 0;
    private int QTD_middleButtonClick = 0;

    //  IMonitorTime
    private Stopwatch timeCounter = new Stopwatch(); // This object allow to count time, to use in logs.

    // Log Classes... I have to implement later...
    //LogController logController = new LogController();

    void Awake()
    {
        this.StartMonitoringTime(); // Conta o tempo de execu��o do script.
        this.InitClickHandler(); 
        UnityEngine.Debug.Log("[+LUDUS-monitor-awake]: O script come�ou no gameobject '" + this.gameObject.name + "',com o id:" + this.gameObject.GetInstanceID()); // Log mostra para o usu�rio que o script est� em a��o!
    }

    void Update()
    {
        //IsPointerNotMoving(); // Fun��o depreciada.
        lastMousePosition = GetMousePosition();
    }
    public Vector3 GetMousePosition() // Retorna a posi��o do Vector3, atrav�s do Input.mousePosition.
    {
        return Input.mousePosition;
    }
    public void OnPointerClick(PointerEventData eventData)
    {

        string side = "undefined"; // String auxiliar utilizada para facilitar o log.

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.ClickOnLeftButton();   // Adiciona cliques ao contador de cliques do bot�o esquerdo.
            side = "esquerdo";          // D� o set identificando qual lado do mouse foi clicado.
                                        // Isso se repete no pr�ximos if's, s� mudando os lados do mouse junto com os contadores.
        }

        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            this.ClickOnMiddleButton();
            side = "do meio";
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            this.ClickOnRightButton();
            side = "direito";
        }

        UnityEngine.Debug.Log("[+LUDUS-mouse-click]: O bot�o " + side + " foi pressionado."); // Mostra o log ao usu�rio.
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        try
        {        
            Vector3 pos = GetMousePosition();
            UnityEngine.Debug.Log("[+LUDUS-mouse-move]: Posi��o atual do cursor:(" + pos.x + "," + pos.y + "," + pos.z + ")");
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
    } // Depreciado
    public void StartMonitoringTime()
    {
        this.timeCounter.Start();       //  Inicia o contador da classe TimeCounter.
    }
    public void EndMonitoringTime()
    {
        this.timeCounter.Stop();         //  Para o contador da classe TimeCounter.
    }
    public TimeSpan GetCurrentTime()
    {
        return timeCounter.Elapsed;
    }
    public void ClickOnRightButton()
    {
        this.QTD_rightButtonClick++;
    }
    public void ClickOnLeftButton()
    {
        this.QTD_leftButtonClick++;
    }
    public void ClickOnMiddleButton()
    {
        this.QTD_middleButtonClick++;
    }
    public void InitClickHandler()
    {
        this.QTD_rightButtonClick = this.QTD_leftButtonClick = this.QTD_middleButtonClick = 0;
    }

}

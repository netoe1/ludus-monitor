using JetBrains.Annotations;
using System;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


// Script feito por Ely Torres Neto para projeto +LUDUS.

// Sobre as variáveis constantes com o prefixo PREFIX:
//      Elas serão usadas como textos padronizados para os logs, isso facilita na hora de tratar erros e dar display nas
//      mensagens, pois é um label padrão, então, se mudarmos uma vez, atualiza no código inteiro.

interface IMonitorMouseData     // Essa interface é responsável por obter os dados do mouse.
{
    Vector3 GetMousePosition(); //  Método Get() que retorna a posição do mouse em coordenadas.
    void IsPointerNotMoving();  //  Verifica se o pointeiro está se mexendo.
    void ClickOnRightButton();  //  Adiciona um clique na váriavel de controle de nro de cliques para o botão direito.
    void ClickOnLeftButton();   //  Adiciona um clique na váriavel de controle de nro de cliques para o botão esquerdo.
    void ClickOnMiddleButton(); //  Adiciona um clique na váriavel de controle de nro de cliques para o botão do meio.
    void InitClickHandler();    //  Inicia o handler de click, ou seja, "inicia a interface", zerando todos os valores que serão utilizados.
}
interface IMonitorTime          //  Essa interface é responsável por gerenciar os contadores de tempo que serão utilizados.
{
    // OBSERVAÇÃO IMPORTANTE: Essas primeiras funções, modificam apenas o TimeSpan e StopWatch do próprio Monitor, o qual já é instanciado por padrão.
    void StartMonitoringTime(); //  Inicia o contador de tempoç.
    TimeSpan GetCurrentTime();  // Obtém data e hora padrão. Similar ao Date.Now() do Javascript.
    void EndMonitoringTime();   // Termina o tempo de monitoramento.
}

public class Monitor :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerMoveHandler,
    IMonitorMouseData,
    IMonitorTime
{


    // Prefixos que serão utilizados de string para os logs.
    const string PREFIX_LUDUS_WARN = "[+LUDUS-WARNING]:";
    const string PREFIX_LUDUS_SUCCESS = "[+LUDUS-SUCCESS]:";
    const string PREFIX_LUDUS_ERROR = "[+LUDUS-ERROR]:";
   
    //  IMonitorMouseData
    private Vector3 lastMousePosition; // Buffer que guarda a ultima posição do mouse.
    // Contadores que armazenam as quantidades de clique em cada botão do mouse.
    private int QTD_rightButtonClick = 0;
    private int QTD_leftButtonClick = 0;
    private int QTD_middleButtonClick = 0;

    //  IMonitorTime
    private Stopwatch timeCounter = new Stopwatch(); // This object allow to count time, to use in logs.

    // Log Classes... I have to implement later...
    //LogController logController = new LogController();

    void Awake()
    {
        this.StartMonitoringTime(); // Conta o tempo de execução do script.
        this.InitClickHandler(); 
        UnityEngine.Debug.Log("[+LUDUS-monitor-awake]: O script começou no gameobject '" + this.gameObject.name + "',com o id:" + this.gameObject.GetInstanceID()); // Log mostra para o usuário que o script está em ação!
    }

    void Update()
    {
        //IsPointerNotMoving(); // Função depreciada.
        lastMousePosition = GetMousePosition();
    }
    public Vector3 GetMousePosition() // Retorna a posição do Vector3, através do Input.mousePosition.
    {
        return Input.mousePosition;
    }
    public void OnPointerClick(PointerEventData eventData)
    {

        string side = "undefined"; // String auxiliar utilizada para facilitar o log.

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.ClickOnLeftButton();   // Adiciona cliques ao contador de cliques do botão esquerdo.
            side = "esquerdo";          // Dá o set identificando qual lado do mouse foi clicado.
                                        // Isso se repete no próximos if's, só mudando os lados do mouse junto com os contadores.
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

        UnityEngine.Debug.Log("[+LUDUS-mouse-click]: O botão " + side + " foi pressionado."); // Mostra o log ao usuário.
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        try
        {        
            Vector3 pos = GetMousePosition();
            UnityEngine.Debug.Log("[+LUDUS-mouse-move]: Posição atual do cursor:(" + pos.x + "," + pos.y + "," + pos.z + ")");
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

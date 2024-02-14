using Ludus.SDK.ExportData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


// Script feito por Ely Torres Neto para projeto +LUDUS.
// Caso voc� queira ver o meu github, fique � vontade: https://github.com/netoe1

// Sobre as vari�veis constantes com o prefixo PREFIX:
//      Elas ser�o usadas como textos padronizados para os logs, isso facilita na hora de tratar erros e dar display nas
//      mensagens, pois � um label padr�o, ent�o, se mudarmos uma vez, atualiza no c�digo inteiro.
// Depreciado

interface IMonitorMouseData     // Essa interface � respons�vel por obter os dados do mouse.
{
    Vector3 GetMousePosition(); //  M�todo Get() que retorna a posi��o do mouse em coordenadas.
   /* void IsPointerNotMoving();*/  //  Verifica se o ponteiro est� se mexendo. Fun��o Depreciada.
}
//interface IMonitorTime          //  Essa interface � respons�vel por gerenciar os contadores de tempo que ser�o utilizados.
//{
//    // OBSERVA��O IMPORTANTE: Essas primeiras fun��es, modificam apenas o TimeSpan e StopWatch do pr�prio Monitor, o qual j� � instanciado por padr�o.
//    void StartMonitoringTime(); //  Inicia o contador de tempo.
//    TimeSpan GetCurrentTime();  // Obt�m data e hora padr�o. Similar ao Date.Now() do Javascript.
//    void EndMonitoringTime();   // Termina o tempo de monitoramento.
//}
//Interface depreciada.

public class Monitor :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerMoveHandler,
    IMonitorMouseData
{
    //// Prefixos que ser�o utilizados de string para os logs.
    //const string PREFIX_LUDUS_WARN = "[+LUDUS-WARNING]:";
    //const string PREFIX_LUDUS_SUCCESS = "[+LUDUS-SUCCESS]:";
    //const string PREFIX_LUDUS_ERROR = "[+LUDUS-ERROR]:";
    // Depreciado.

    ////  IMonitorTime
    //private Stopwatch timeCounter = new Stopwatch(); // This object allow to count time, to use in logs.
    // Depreciado.

    private LudusLog log = new LudusLog("Monitor-exec","Execu��o do m�dulo monitor.");

    void Start()
    {
        log.addCol(new LudusLogCol("[+LUDUS-monitor-start]", "O script iniciou em'" + this.gameObject.name + "', id: " + this.gameObject.GetInstanceID()));
      //  UnityEngine.Debug.Log("[+LUDUS-monitor-start]: O script iniciou em '" + this.gameObject.name + "',id:" + this.gameObject.GetInstanceID()); // Log mostra para o usu�rio que o script est� em a��o!
      DontDestroyOnLoad(FindObjectOfType<Canvas>());
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
            side = "esquerdo";          // D� o set identificando qual lado do mouse foi clicado.
        }

        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
           
            side = "do meio";
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            side = "direito";
        }
        log.addCol(new LudusLogCol("[+LUDUS-mouse-click]:", "Bot�o " + side + " ."));
      //  UnityEngine.Debug.Log("[+LUDUS-mouse-click]: O bot�o " + side + "."); // Mostra o log ao usu�rio.
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        try
        {        
            Vector3 pos = GetMousePosition();
            log.addCol(new LudusLogCol("[+LUDUS-mouse-move]:", "POS:(" + pos.x + ";" + pos.y + ";" + pos.z + ")"));
          //  UnityEngine.Debug.Log("[+LUDUS-mouse-move]: POS:(" + pos.x + "," + pos.y + "," + pos.z + ")");
        }
        catch (UnityException err)
        {
            throw err;
        }
    }


    //public void IsPointerNotMoving()
    //{
    //    if (lastMousePosition == GetMousePosition())
    //    {       
    //        UnityEngine.Debug.Log("[monitor-mouse-not-move]: The pointer is in the same position.");
    //    }

    //    lastMousePosition = GetMousePosition();
    //} // Depreciado

    #region SCENE_SCRIPT_MANAGEMENT
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Cena carregada: " + scene.name);
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Cena descarregada: " + scene.name);
        this.log.addCol(new LudusLogCol("Descarregando cena",""));
        this.log.export();
        this.log.reset();
        Destroy(this.gameObject);
    }
    #endregion SCENE_SCRIPT_MANAGEMENT
}   

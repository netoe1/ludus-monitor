using Ludus.SDK.ExportData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


// Script feito por Ely Torres Neto para projeto +LUDUS.
// Caso você queira ver o meu github, fique à vontade: https://github.com/netoe1

// Sobre as variáveis constantes com o prefixo PREFIX:
//      Elas serão usadas como textos padronizados para os logs, isso facilita na hora de tratar erros e dar display nas
//      mensagens, pois é um label padrão, então, se mudarmos uma vez, atualiza no código inteiro.
// Depreciado

interface IMonitorMouseData     // Essa interface é responsável por obter os dados do mouse.
{
    Vector3 GetMousePosition(); //  Método Get() que retorna a posição do mouse em coordenadas.
   /* void IsPointerNotMoving();*/  //  Verifica se o ponteiro está se mexendo. Função Depreciada.
}
//interface IMonitorTime          //  Essa interface é responsável por gerenciar os contadores de tempo que serão utilizados.
//{
//    // OBSERVAÇÃO IMPORTANTE: Essas primeiras funções, modificam apenas o TimeSpan e StopWatch do próprio Monitor, o qual já é instanciado por padrão.
//    void StartMonitoringTime(); //  Inicia o contador de tempo.
//    TimeSpan GetCurrentTime();  // Obtém data e hora padrão. Similar ao Date.Now() do Javascript.
//    void EndMonitoringTime();   // Termina o tempo de monitoramento.
//}
//Interface depreciada.

public class Monitor :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerMoveHandler,
    IMonitorMouseData
{
    //// Prefixos que serão utilizados de string para os logs.
    //const string PREFIX_LUDUS_WARN = "[+LUDUS-WARNING]:";
    //const string PREFIX_LUDUS_SUCCESS = "[+LUDUS-SUCCESS]:";
    //const string PREFIX_LUDUS_ERROR = "[+LUDUS-ERROR]:";
    // Depreciado.

    ////  IMonitorTime
    //private Stopwatch timeCounter = new Stopwatch(); // This object allow to count time, to use in logs.
    // Depreciado.

    private LudusLog log = new LudusLog("Monitor-exec","Execução do módulo monitor.");

    void Start()
    {
        log.addCol(new LudusLogCol("[+LUDUS-monitor-start]", "O script iniciou em'" + this.gameObject.name + "', id: " + this.gameObject.GetInstanceID()));
      //  UnityEngine.Debug.Log("[+LUDUS-monitor-start]: O script iniciou em '" + this.gameObject.name + "',id:" + this.gameObject.GetInstanceID()); // Log mostra para o usuário que o script está em ação!
      DontDestroyOnLoad(FindObjectOfType<Canvas>());
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
            side = "esquerdo";          // Dá o set identificando qual lado do mouse foi clicado.
        }

        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
           
            side = "do meio";
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            side = "direito";
        }
        log.addCol(new LudusLogCol("[+LUDUS-mouse-click]:", "Botão " + side + " ."));
      //  UnityEngine.Debug.Log("[+LUDUS-mouse-click]: O botão " + side + "."); // Mostra o log ao usuário.
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

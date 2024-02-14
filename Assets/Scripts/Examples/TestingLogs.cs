using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludus.SDK.ExportData;

public class TestingLogs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LudusLog log = new LudusLog("Teste1232323232!","Primeiro log.");
        log.addCol(new LudusLogCol("[Ludus-click]","Ocorreu um clique no mouse."));
        log.addCol(new LudusLogCol("[Ludus-click]", "Ocorreu um clique no mouse."));
        log.addCol(new LudusLogCol("[Ludus-click]", "Ocorreu um clique no mouse."));
        log.addCol(new LudusLogCol("[Ludus-click]", "Ocorreu um clique no mouse."));
        log.export();
    } 
}

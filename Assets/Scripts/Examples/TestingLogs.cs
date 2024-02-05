using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludus.SDK.ExportData;

public class TestingLogs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreateLog log = new CreateLog("Teste1232323232!","Primeiro log.");
        log.addCell(new LogCell("[Ludus-click]","Ocorreu um clique no mouse."));
        log.addCell(new LogCell("[Ludus-click]", "Ocorreu um clique no mouse."));
        log.addCell(new LogCell("[Ludus-click]", "Ocorreu um clique no mouse."));
        log.addCell(new LogCell("[Ludus-click]", "Ocorreu um clique no mouse."));
        log.exportLog();
    } 
}

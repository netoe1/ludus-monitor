using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ludus.sdk.exportData;

public class TestingLogs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LogCell logCell = new LogCell("Testando.","Primeiro Log de teste!");
        LogCell logCell2 = new LogCell("Segundo teste....","Segundo teste!");
        CreateLog createLog = new CreateLog("Opa","Fazendo o primeiro log!");
        createLog.addLogCell(logCell);
        createLog.addLogCell(logCell2);
        createLog.exportLog();
    }

 
}

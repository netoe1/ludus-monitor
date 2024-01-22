using System;
using System.Collections.Generic;
using UnityEngine;

// Basicamente, utilizando como conceito primordial as tabelas de banco de dados, o nosso LOG Geral ser� um grande tabela, com v�rios logs.
// A classe LogCell seria uma Coluna; logController seria a tabela.
// Ent�o, cada c�lula do log, teria um id, t�tulo, descri��o e o tempo de execu��o em que foi executado.
// Depois, esses dados ser�o adicionados no LogController e assim ser�o submetidos.
// Por enquanto, eles est�o em formato de string, mas pretendemos coloc�-los em JSON, e assim exportar para um banco de dados.




public class LogCell 
{
    private string id;
    private string title;
    private string description;
    private string date;
    private TimeSpan timeElapsed;

    public LogCell(string __title, string __description, string __date, TimeSpan timeElapsed)
    {
        try
        {
            if (__title != null && __description != null && __date == null)
            {
                this.id = "To set.";
                this.title = __title;
                this.description = __description;
                this.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                this.timeElapsed = timeElapsed;
                return;
            }

            throw new UnityException("[monitor-log-cell-err]:Error to add an LogCell");
        }
        catch (UnityException err)
        {
            throw err;
        }


    }

    public override string ToString()
    {
        return "Title:" + this.title + "\nDescription:" + this.description + "\nDate:" + this.date + "\n";
    }

    public string getId()
    {
        return this.id;
    }
    public void setId(string id)
    {
        try
        {
            if (id != null)
            {
                this.id = id;
                return;
            }

            throw new UnityException("[monitor-logcell-setId-err]: You provide an invalid id.");
        }
        catch (UnityException err)
        {
            throw err;
        }

    }

    public string getTitle()
    {
        return this.title;
    }
    public void setTitle(string title)
    {
        try
        {
            if (title != null)
            {
                this.title = title;
                return;
            }

            throw new UnityException("[monitor-logcell-setTitle-err]: You provide an invalid title to set.");
        }
        catch (UnityException err)
        {
            throw err;
        }
    }

    public string getDescription()
    {
        return this.description;
    }

    public void setDescription(string description)
    {
        try
        {
            if (description != null)
            {
                this.description = description;
                return;
            }

            throw new UnityException("[monitor-logcell-setDescription-err]: You provide an invalid description to set.");
        }
        catch (UnityException err)
        {
            throw err;
        }
    }
    public TimeSpan getTimeElapsed()
    {
        return this.timeElapsed;
    }

}
public class LogController
{
    private List<LogCell> logCells = new List<LogCell>(); // Cria uma lista de colunas;
    private int counter = 0; // Contador de colunas.
    private string title;
    private string description;
    

    //  OBS: Esse contador � uma alternativa mais eficiente, pois ele ir� sempre ser adicionado, em vez de contar os elementos da lista.
    // Tamb�m, este contador deve ser imut�vel, ele ser� utilizado com id. Por execu��o, ele deve ser un�co. Ent�o, caso ocorra algum erro, ele ser� adicionado automaticamente
    // e o �ndice onde o erro ocorreu ser� anulado.

    public void addLogCell(LogCell newLog)
    {
        try
        {
            if (newLog == null)
            {
                throw new UnityException("[+LUDUS-logcontroller-err]: Voc� deve prover uma c�lula de log para submeter.");
            }
            counter++;
            newLog.setId(counter.ToString());
            logCells.Add(newLog);

        }
        catch (UnityException err)
        {
            throw err;
        }
    }

    public void removeLogCellById(string id)
    {
        try
        {
            LogCell logToRemove = logCells.Find(log => log.getId() == id);

            if (logToRemove != null)
            {
                logCells.Remove(logToRemove);
            }
            else
            {
                throw new UnityException("[+LUDUS-logcontroller-err]: O logCell que voc� informou, n�o foi encontrado.");
            }
        }
        catch (UnityException err)
        {
            throw err;
        }
    }

    public void editLogCellById(string id, string newTitle, string newDescription)
    {
        try
        {
            LogCell logToEdit = logCells.Find(log => log.getId() == id);

            if (logToEdit != null)
            {
                logToEdit.setTitle(newTitle);
                logToEdit.setDescription(newDescription);
            }
            else
            {
                throw new UnityException("[+LUDUS-logcontroller-err]: N�o foi poss�vel encontrar o logCell com o id '" + id + "' para edi��o.");
            }
        }
        catch (UnityException err)
        {
            throw err;
        }
    }

    public void exportLog(string id)
    {
        try
        {
            LogCell logToExport = logCells.Find(log => log.getId() == id);

            if (logToExport != null)
            {
                Debug.Log("Exporting log:\n" + logToExport.ToString());
            }
            else
            {
                throw new UnityException("[+LUDUS-logcontroller-err]: N�o foi poss�vel encontrar o logCell com o id '" + id + "' para exportar.");
            }
        }
        catch (UnityException err)
        {
            throw err;
        }
    }


}

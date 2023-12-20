using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;


// Log types is an other idea.

    //enum log_types
    //{
    //    time,
    //    click,
    //    button
    //};


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
    private List<LogCell> logCells = new List<LogCell>();
    private int counter = 0;
    public void addLogCell(LogCell newLog)
    {
        try
        {
            if (newLog == null)
            {
                throw new UnityException("[monitor-logController-addLogCell-err]: You must provide a LogCell object in addAnLogCell() to create a log.");
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
                throw new UnityException("[monitor-logController-removeLogCell-err]: LogCell with id " + id + " not found.");
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
                throw new UnityException("[monitor-logController-editLogCell-err]: LogCell with id " + id + " not found.");
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
               // Implement later.
                Debug.Log("Exporting log:\n" + logToExport.ToString());
            }
            else
            {
                throw new UnityException("[monitor-logController-exportLog-err]: LogCell with id " + id + " not found.");
            }
        }
        catch (UnityException err)
        {
            throw err;
        }
    }


}

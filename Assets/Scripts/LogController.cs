using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.PackageManager.Requests;
using UnityEngine;


public class LogCell
{
    private string id;
    private string title;
    private string description;
    private string date;



    public LogCell(string __title, string __description, string __date)
    {
        try
        {
            if (__title != null && __description != null && __date == null)
            {
                this.id = "To set.";
                this.title = __title;
                this.description = __description;
                this.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
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

    public void setId(string id)
    {
        try
        {
            if (id == null)
            {
                throw new UnityException("[monitor-logCell-err]: You must provide an valid id to set.");
            }

            this.id = id;
        }
        catch (UnityException err)
        {
            throw err;
        }
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
                throw new UnityException("[monitor-logController-err]: You must provide a LogCell object in addAnLogCell() to create a log.");
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

    }

    public void editLogCellById(string id)
    {

    }

    public void exportLog(string id)
    {

    }


}

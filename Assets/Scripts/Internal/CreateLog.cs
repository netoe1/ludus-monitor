using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Ludus.SDK.Utils;

// Basicamente, utilizando como conceito primordial as tabelas de banco de dados, o nosso LOG Geral ser� um grande tabela, com v�rios logs.
// A classe LogCell seria uma Coluna; logController seria a tabela.
// Ent�o, cada c�lula do log, teria um id, t�tulo, descri��o e o tempo de execu��o em que foi executado.
// Depois, esses dados ser�o adicionados no LogController e assim ser�o submetidos.
// Por enquanto, eles est�o em formato de string, mas pretendemos coloc�-los em JSON, e assim exportar para um banco de dados.


namespace Ludus.SDK.ExportData
{
    #region LOG_CELL 
    // Criando a classe LogCell, que seriam as colunas da nossa tabela CreateLog.
    // Elas possuem alguns atributos.
    /*
        1   - Id: � o id da tabela a ser gerado, normalmente s�o representados por um n�mero inteiro em formato de string.
        2   - Title: T�tulo da c�lula, pode ser qualquer coisa, mas normalmente para um boa pr�tica, recomendamos colocar um t�tulo que seja 
            correspondente ao uso.
        3   - Data: S�o as informa��es sobre o log. Voc� pode covert�-los a um JSON string, e colocar ali. Nesta vers�o, por enquanto, s� h� esta op��o.
        4   - Date: Pega a description e hora do momento e registra, � feito de foram autom�tica, voc� nem precisa se preocupar  com isso.
        5   - TimeElapsed: � um atributo que diz respeito ao tempo de execu��o do script.
     */
    [System.Serializable]
    public class LogCell
    {
        public string id;
        public string title;
        public string description;
        public string date;
        public LogCell(string title = "", string description = "")
        {
            try
            {
                if (title == "")
                {
                    throw new UnityException("[LogCell-module-cell-err]: O t�tulo deve ser obrigat�rio. ");
                }

                if (description == "")
                {
                    description = "N�o informada";
                }

                this.id = "To set.";
                this.title = title;
                this.description = description;
                this.date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            }
            catch (UnityException err)
            {
                throw err;
            }
        }
    }
    #endregion LOG_CELL

    #region CREATE_LOG
    /*
        O CreateLog � feito para abrigar todas as colunas em apenas uma tabela; abrigar todos os objetos em um objeto-pai, para termos os dados de forma centralizada.
        Sobre os atributos:
        Lista LogCells - � a lista das c�lulas de log.
        Counter - serve para contar a quantidade de colunas, sem precisar usar reports.Count toda hora.
        Title - T�tulo do log pai.
        Data - Feito para abrigar os dados, representado de em string.
     */
    [System.Serializable]
    public class CreateLog
    {
        public string title; // T�tulo do log.
        public string description;
        public List<LogCell> reports = new List<LogCell>(); // Cria uma lista de colunas;
        private int counter = 0; // Contador de colunas.

        //  OBS: Esse contador � uma alternativa mais eficiente, pois ele ir� sempre ser adicionado, em vez de contar os elementos da lista.
        // Tamb�m, este contador deve ser imut�vel, ele ser� utilizado com id. Por execu��o, ele deve ser un�co. Ent�o, caso ocorra algum erro, ele ser� adicionado automaticamente
        // e o �ndice onde o erro ocorreu ser� anulado.

        public CreateLog(string title = "", string description = "")
        {
            try
            {
                if (title == "")
                {
                    throw new UnityException("[+LUDUS-createlog-object-err]:O t�tulo � obrigat�rio.");
                }

                if (description == "")
                {
                    description = "N�o informada.";
                }

                this.title = title;
                this.description = description;

            }
            catch (UnityException err)
            {
                throw err;
            }
        }

        public void addCell(LogCell newLog)
        {
            try
            {
                if (newLog == null)
                {
                    throw new UnityException("[+LUDUS-createlog-err]: A c�lula de log est� vazia.");
                }
                counter++;
                newLog.id = counter.ToString();
                reports.Add(newLog);

            }
            catch (UnityException err)
            {
                throw err;
            }
        }

        public void removeCellById(string id)
        {
            try
            {
                LogCell logToRemove = reports.Find(log => log.id == id);

                if (logToRemove != null)
                {
                    reports.Remove(logToRemove);
                }
                else
                {
                    throw new UnityException("[+LUDUS-createlog-err]: c�lula n�o encontrada.");
                }
            }
            catch (UnityException err)
            {
                throw err;
            }
        }

        //public void editLogCellById(string id, string newTitle, string newDescription)
        //{
        //    try
        //    {
        //        LogCell logToEdit = reports.Find(log => log.id == id);

        //        if (logToEdit != null)
        //        {
        //            logToEdit.title = newTitle;
        //            logToEdit.description = newDescription;
        //        }
        //        else
        //        {
        //            throw new UnityException("[+LUDUS-createlog-err]:C�lula com o id '" + id + "' para edi��o.");
        //        }
        //    }
        //    catch (UnityException err)
        //    {
        //        throw err;
        //    }
        //} // Depreciado
        public void exportLog()
        {
            try
            {
                string jsonToExport = "";
                this.addCell(new LogCell("Exporta��o de arquivo.", "Exportando JSON."));
                jsonToExport = JsonUtility.ToJson(this);

                string rootFolder = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
                if (rootFolder == null)
                {
                    throw new UnityException("[+LUDUS-createlog-exportJSON-err]: Diret�rio n�o encontrado.");
                }

                string directoryPath = Path.Combine(rootFolder, "Assets", "Resources");
                string filename = this.title + "-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "-"+ Utils.Utils.genRandomNumAsString(3)  + ".json";
                string filePath = Path.Combine(directoryPath, filename);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(jsonToExport);
                }

                Debug.Log("Arquivo exportado com sucesso: " + filePath);
            }
            catch (Exception err)
            {
                Debug.LogError("Erro ao exportar o arquivo: " + err.Message);
            }
        }

        public void reset()
        {
            this.reports.Clear();
        }

        public void redefine(string title = "", string description = "")
        {
            try
            {
                if (title == "")
                {
                    throw new UnityException("[create-log-module-err]:O t�tulo � um par�metro obrigat�rio.");
                }

                if (description == "")
                {
                    description = "N�o informada";
                }

                this.title = title;
                this.description = description;
            }
            catch (UnityException err)
            {
                throw err;
            }
        }
    }
    #endregion CREATE_LOG
};

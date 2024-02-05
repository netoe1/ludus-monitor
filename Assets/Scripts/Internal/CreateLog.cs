using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Ludus.SDK.Utils;

// Basicamente, utilizando como conceito primordial as tabelas de banco de dados, o nosso LOG Geral será um grande tabela, com vários logs.
// A classe LogCell seria uma Coluna; logController seria a tabela.
// Então, cada célula do log, teria um id, título, descrição e o tempo de execução em que foi executado.
// Depois, esses dados serão adicionados no LogController e assim serão submetidos.
// Por enquanto, eles estão em formato de string, mas pretendemos colocá-los em JSON, e assim exportar para um banco de dados.


namespace Ludus.SDK.ExportData
{
    #region LOG_CELL 
    // Criando a classe LogCell, que seriam as colunas da nossa tabela CreateLog.
    // Elas possuem alguns atributos.
    /*
        1   - Id: é o id da tabela a ser gerado, normalmente são representados por um número inteiro em formato de string.
        2   - Title: Título da célula, pode ser qualquer coisa, mas normalmente para um boa prática, recomendamos colocar um título que seja 
            correspondente ao uso.
        3   - Data: São as informações sobre o log. Você pode covertê-los a um JSON string, e colocar ali. Nesta versão, por enquanto, só há esta opção.
        4   - Date: Pega a description e hora do momento e registra, é feito de foram automática, você nem precisa se preocupar  com isso.
        5   - TimeElapsed: é um atributo que diz respeito ao tempo de execução do script.
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
                    throw new UnityException("[LogCell-module-cell-err]: O título deve ser obrigatório. ");
                }

                if (description == "")
                {
                    description = "Não informada";
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
        O CreateLog é feito para abrigar todas as colunas em apenas uma tabela; abrigar todos os objetos em um objeto-pai, para termos os dados de forma centralizada.
        Sobre os atributos:
        Lista LogCells - é a lista das células de log.
        Counter - serve para contar a quantidade de colunas, sem precisar usar reports.Count toda hora.
        Title - Título do log pai.
        Data - Feito para abrigar os dados, representado de em string.
     */
    [System.Serializable]
    public class CreateLog
    {
        public string title; // Título do log.
        public string description;
        public List<LogCell> reports = new List<LogCell>(); // Cria uma lista de colunas;
        private int counter = 0; // Contador de colunas.

        //  OBS: Esse contador é uma alternativa mais eficiente, pois ele irá sempre ser adicionado, em vez de contar os elementos da lista.
        // Também, este contador deve ser imutável, ele será utilizado com id. Por execução, ele deve ser uníco. Então, caso ocorra algum erro, ele será adicionado automaticamente
        // e o índice onde o erro ocorreu será anulado.

        public CreateLog(string title = "", string description = "")
        {
            try
            {
                if (title == "")
                {
                    throw new UnityException("[+LUDUS-createlog-object-err]:O título é obrigatório.");
                }

                if (description == "")
                {
                    description = "Não informada.";
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
                    throw new UnityException("[+LUDUS-createlog-err]: A célula de log está vazia.");
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
                    throw new UnityException("[+LUDUS-createlog-err]: célula não encontrada.");
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
        //            throw new UnityException("[+LUDUS-createlog-err]:Célula com o id '" + id + "' para edição.");
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
                this.addCell(new LogCell("Exportação de arquivo.", "Exportando JSON."));
                jsonToExport = JsonUtility.ToJson(this);

                string rootFolder = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
                if (rootFolder == null)
                {
                    throw new UnityException("[+LUDUS-createlog-exportJSON-err]: Diretório não encontrado.");
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
                    throw new UnityException("[create-log-module-err]:O título é um parâmetro obrigatório.");
                }

                if (description == "")
                {
                    description = "Não informada";
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

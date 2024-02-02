using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

// Basicamente, utilizando como conceito primordial as tabelas de banco de dados, o nosso LOG Geral será um grande tabela, com vários logs.
// A classe LogCell seria uma Coluna; logController seria a tabela.
// Então, cada célula do log, teria um id, título, descrição e o tempo de execução em que foi executado.
// Depois, esses dados serão adicionados no LogController e assim serão submetidos.
// Por enquanto, eles estão em formato de string, mas pretendemos colocá-los em JSON, e assim exportar para um banco de dados.


namespace Ludus.sdk.exportData
{
    #region LOG_CELL 
    // Criando a classe LogCell, que seriam as colunas da nossa tabela CreateLog.
    // Elas possuem alguns atributos.
    /*
        1   - Id: é o id da tabela a ser gerado, normalmente são representados por um número inteiro em formato de string.
        2   - Title: Título da célula, pode ser qualquer coisa, mas normalmente para um boa prática, recomendamos colocar um título que seja 
            correspondente ao uso.
        3   - Data: São as informações sobre o log. Você pode covertê-los a um JSON string, e colocar ali. Nesta versão, por enquanto, só há esta opção.
        4   - Date: Pega a data e hora do momento e registra, é feito de foram automática, você nem precisa se preocupar  com isso.
        5   - TimeElapsed: é um atributo que diz respeito ao tempo de execução do script.
     */
    [System.Serializable]
    public class LogCell
    {
        public string id;
        public string title;
        public string data;
        public string date;

        public LogCell(string __title, string __description)
        {
            try
            {
                if (__title != null && __description != null)
                {
                    this.id = "To set.";
                    this.title = __title;
                    this.data = __description;
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
            return "Title:" + this.title + "\nDescription:" + this.data + "\nDate:" + this.date + "\n";
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
        public List<LogCell> reports = new List<LogCell>(); // Cria uma lista de colunas;
        private int counter = 0; // Contador de colunas.
        public string title; // Título do log.
        public string data;


        //  OBS: Esse contador é uma alternativa mais eficiente, pois ele irá sempre ser adicionado, em vez de contar os elementos da lista.
        // Também, este contador deve ser imutável, ele será utilizado com id. Por execução, ele deve ser uníco. Então, caso ocorra algum erro, ele será adicionado automaticamente
        // e o índice onde o erro ocorreu será anulado.

        public CreateLog(string title = "",string data = "")
        {
            try
            {
                if(title == "")
                {
                    throw new UnityException("[+LUDUS-createlog-object-err]:O título é obrigatório para a criação do objeto.");
                }

                if(data == "")
                {
                    data = "Não informada.";
                }

                this.title = title;
                this.data = data;

            }
            catch(UnityException err)
            {
                throw err;
            }
        }

        public void addLogCell(LogCell newLog)
        {
            try
            {
                if (newLog == null)
                {
                    throw new UnityException("[+LUDUS-createlog-err]: Você deve prover uma célula de log para submeter.");
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

        public void removeLogCellById(string id)
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
                    throw new UnityException("[+LUDUS-createlog-err]: O logCell que você informou, não foi encontrado.");
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
                LogCell logToEdit = reports.Find(log => log.id == id);

                if (logToEdit != null)
                {
                    logToEdit.title = newTitle;
                    logToEdit.data = newDescription;
                }
                else
                {
                    throw new UnityException("[+LUDUS-createlog-err]: Não foi possível encontrar o logCell com o id '" + id + "' para edição.");
                }
            }
            catch (UnityException err)
            {
                throw err;
            }
        }
        public void exportLog()
        {
            try
            {
                string jsonToExport = "";

                jsonToExport = JsonUtility.ToJson(this);

                string rootFolder = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
                if (rootFolder == null)
                {
                    throw new UnityException("[+LUDUS-createlog-exportJSON-err]: Erro ao obter os diretórios da pasta padrão do JSON.");
                }

                string directoryPath = Path.Combine(rootFolder, "Assets", "Reports");
                string filename = this.title + "-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".json";
                string filePath = Path.Combine(directoryPath, filename);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {             
                    writer.WriteLine(jsonToExport);
                }

                Debug.Log("Arquivo exportado com sucesso em: " + filePath);
            }
            catch (Exception err)
            {
                Debug.LogError("Erro ao exportar o arquivo: " + err.Message);
            }
        }


    }
    #endregion CREATE_LOG
};

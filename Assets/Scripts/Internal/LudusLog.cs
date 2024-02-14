using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Ludus.SDK.Utils;

// Basicamente, utilizando como conceito primordial as tabelas de banco de dados, o nosso LOG Geral será um grande tabela, com vários logs.
// A classe LudusLogCol seria uma Coluna; logController seria a tabela.
// Então, cada célula do log, teria um id, título, descrição e o tempo de execução em que foi executado.
// Depois, esses dados serão adicionados no LogController e assim serão submetidos.
// Por enquanto, eles estão em formato de string, mas pretendemos colocá-los em JSON, e assim exportar para um banco de dados.


namespace Ludus.SDK.ExportData
{
    #region LUDUSLOG_COL 
    // Criando a classe LudusLogCol, que seriam as colunas da nossa tabela LudusLog.
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
    public class LudusLogCol
    {
        public string id;
        public string title;
        public string description;
        public string date;
        public LudusLogCol(string title = "", string description = "")
        {
            try
            {
                if (title == "")
                {
                    throw new UnityException("[LudusLogCol-module-cell-err]: O título deve ser obrigatório. ");
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
    #endregion LUDUSLOG_COL

    #region LUDUS_LOG
    /*
        O LudusLog é feito para abrigar todas as colunas em apenas uma tabela; abrigar todos os objetos em um objeto-pai, para termos os dados de forma centralizada.
        Sobre os atributos:
        Lista LogCells - é a lista das células de log.
        Counter - serve para contar a quantidade de colunas, sem precisar usar reports.Count toda hora.
        Title - Título do log pai.
        Data - Feito para abrigar os dados, representado de em string.
     */
    [System.Serializable]
    public class LudusLog
    {
        public string title; // Título do log.
        public string description;
        public List<LudusLogCol> reports = new List<LudusLogCol>(); // Cria uma lista de colunas;
        private int counter = 0; // Contador de colunas.

        //  OBS: Esse contador é uma alternativa mais eficiente, pois ele irá sempre ser adicionado, em vez de contar os elementos da lista.
        // Também, este contador deve ser imutável, ele será utilizado com id. Por execução, ele deve ser uníco. Então, caso ocorra algum erro, ele será adicionado automaticamente
        // e o índice onde o erro ocorreu será anulado.

        public LudusLog(string title = "", string description = "") // Construtor do LudusLog...
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
        public void addCol(LudusLogCol newLog) // Adicionar uma coluna...
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
        public void removeColById(string id) // Remover uma coluna pelo seu id.
        {
            try
            {
                LudusLogCol logToRemove = reports.Find(log => log.id == id);

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
        public void export() // Exportar o log.
        {
            try
            {
                string jsonToExport = "";
                this.addCol(new LudusLogCol("Exportação de arquivo.", "Exportando JSON."));
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
        public void reset() // Resetar os dados do log, mas manter o seu título e id padrão,
        {
            this.reports.Clear();
        }
        public void redefine(string title = "", string description = "") // Redefinir todo o log, desde os seus dados, até o seu título e descrição, reaproveitando o objeto.
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
    #endregion LUDUS_LOG
};

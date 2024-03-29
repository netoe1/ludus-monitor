# ludus-monitor: A module from +LUDUS Project.

<h3>Sobre o projeto:</h3>
<p>
  Ludus-Monitor é uma dependência do projeto +LUDUS e é responsável por coletar dados brutos do usuário, como cliques, movimento do mouse e
  tempo percorrido, nos jogos +LUDUS. Estes dados não são usados de forma indevida, e sim, utilizados como base nos projetos acadêmicos, para avaliar a eficiência dos nossos jogos.
  As nossas produções academicas, estão em fase de pesquisa e desenvolvimento e servem de auxílio ao ensino, atendendendo à educação com uma abordagem lúdica e divertida.
  <br>
  <i>
    <strong>Para mais informações</strong>, acesse o <a href = "https://sites.google.com/view/maisludus">site do projeto</a>, que possui uma parceria entre IFSul Câmpus Bagé e Pelotas.
  </i>
</p>

<h1>Início da documentação:</h1>
<p>Sobre este projeto da Unity, utilizaremos dois principais tipos de script. Primeiro, <strong>Monitor.cs</strong>, que é adicionado aos gameObjects que queremos escutar.
  e o <strong>LudusLog (antigo LudusLog)</strong>, responsável por gerar logs em formato .JSON.
  Utilizando dos eventos disponíveis ao desenvolvedor com o namespace UnityEngine.EventSystems, conseguimos captar alguns dados brutos através da unity.
  <br>
  Estes dados brutos, serão:
  <ul>
    <li>Posições do mouse, acionadas quando ele se move.</li>
    <li>Cliques do mouse, em relação ao gameObject em que o script foi atrelado.</li>
    <li>Criação de logs em formato JSON.</li>
    <li>Data e hora em que estes eventos foram acionados pela engine.</li>
  </ul>
  
  <strong>OBS:</strong> <i>Algumas interfaces são criadas pelo desenvolvedor, com a função de <strong>abstrair ou facilitar</strong> a implementação de alguns métodos, focando em sua portabilidade.</i>
  <br>

  <h1>Monitor.cs</h1>
  
  ```csharp
using Ludus.SDK.ExportData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
// O monitor não tem muito mistério, você não precisa saber muito sobre a sua implementação, mas o mais importante é anexá-lo a um gameobject.
// É só isso que precisa saber; nós procuramos deixar sua implementação tratar o maior número de erros possível,
// para não estourar um Debug.LogError automático do sistema.
// O monitor usa o LudusLog para exportar as informações. Tirando isso, ele é autossuficiente.

public class Monitor :
MonoBehaviour,
IPointerClickHandler,
IPointerMoveHandler,
IMonitorMouseData
{

    private LudusLog log = new LudusLog("Monitor-exec","Execução do módulo monitor.");
    void Start();
    public Vector3 GetMousePosition(); // Retorna a posição, em formato Vector3 (x,y,z), através do Input.mousePosition.
    public void OnPointerClick(PointerEventData eventData); // Evento da unity que monitora todos os cliques no gameobject atrelado.
    public void OnPointerMove(PointerEventData eventData); // Evento da unity que monitora todos os movimentos do mouse naquele gameobject atrelado.

    #region SCENE_SCRIPT_MANAGEMENT
    // Essa região fala sobre os controles de cena customizados, pois estamos modificando . Neste caso, são métodos privados da cena, pois estes estão relacionados com o bom funcionamento do script. Você não deve usar nenhum destes métodos.
    void OnEnable();
    void OnDisable();
    void OnSceneLoaded(Scene scene, LoadSceneMode mode);
    void OnSceneUnloaded(Scene scene);
    #endregion SCENE_SCRIPT_MANAGEMENT

}

````

</p>

<h1>Falando sobre o LudusLog (parte mais crucial)</h1>
<p>O <strong>LudusLog (LudusLog)</strong> é uma classe .cs, pensada em uma <strong>arquitetura portátil</strong> e não só mais um módulo exclusivo do Framework.
<strong>Dentro do projeto</strong>, LudusLog é uma classe pública que pode ser instanciada em qualquer script, então sinta-se em casa.
</p>

<h3>Visão geral do script:</h3>
<p>

```csharp

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Ludus.SDK.Utils;

// Basicamente, utilizando como conceito primordial as tabelas de banco de dados, o nosso LOG Geral será um grande tabela, com vários logs;
// A classe LudusLogCol seria uma Coluna da Tabela Mãe LudusLog;
// Então, cada célula do log, teria um id, título e descrição;
// Depois, esses dados serão adicionados no LudusLog e assim serão submetidos com o método de exportar.
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
      3   - Description São as informações sobre o log. Você pode covertê-los a um JSON string, e colocar ali; ou usar como uma string mesmo, como você preferir.
      4   - Date: Pega a data e hora do momento e registra, é feito de foram automática, você nem precisa se preocupar com isso.
   */
  [System.Serializable]
  public class LudusLogCol
  {
      public string id; // ID da célula
      public string title; // Título da célula
      public string description; // Descrição da célula
      public string date; // Data e hora
      public LudusLogCol(string title = "", string description = ""); // Construtor.
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
      public string description; // Descrição do log
      public List<LudusLogCol> reports = new List<LudusLogCol>(); // Cria uma lista de colunas;
      private int counter = 0; // Contador de colunas, uso da própria API.

      public LudusLog(string title = "", string description = ""); // Construtor
      public void addCol(LudusLogCol newLog); // Adicionar uma célula (coluna)
      public void removeColById(string id); // Remover uma célula pelo seu id
      public void export(); // Exportar o log.
      public void reset(); // Resetar o objeto
      public void redefine(string title = "", string description = ""); // Redefinir configurações, sem apagar o conteúdo antigo, como os contadores.
  }
  #endregion LUDUS_LOG
};
````

</p>

<h1>LudusLog, na prática:</h1>
<p>
  
  ```csharp
  
  using UnityEngine;
  using System;
  using System.Generic.Collections;
  using System;
  using Ludus.Sdk.ExportData; // Lembre-se de usar o namespace do framework...

public class Foo: MonoBehavior
{
void Start()
{
// Assim como uma tabela, você deve adicionar as colunas.
// Para isso, primeiro você precisa instanciar o objeto.
// O construtor de LudusLog aceita dois argumentos principais: new LudusLog (string title, string description);
// O título é obrigatório, pois é o que identifica o log; a descrição não é necessária, mas recomendamos pois deixa o uso mais organizado.

          LudusLog log = new LudusLog ("Meu primeiro Log","Minha descrição");


          // Agora, você precisar criar as colunas, ou seja, os dados que serão colocados nesta tabela, então para isso, você vai usar o método LudusLog.addCol();
          // Você pode criar em um objeto separado ou instanciar diretamente no parâmetro da função.
          // Iniciando diretamente no parâmetro da função: new LudusLogCol(string title,string description);

          log.addCol(new LudusLogCol("Número de cliques","10"));
          log.addCol(new LudusLogCol("Posição do mouse","POS:(1,10,0)"));

          //  Ou
          LudusLogCol novaColuna = new LudusLogCol("coluna nova","forma separada");
          log.addCol(novaColuna);

          // Você adicionou com sucesso uma nova coluna!

          // Você consegue remove o log, porém, é necessário saber o seu id. Você consegue saber o id se fez de forma separada,
          // sendo uma vantagem a quem separa o log. Porém, o código fica mais visualmente poluído, você decide qual estratégia quer usar.

          log.removeColById(novaColuna.id); // Caso esse id exista, ele será removido, caso contrário, ocorrerá uma mensagem de erro.

          // Agora, para transformá-lo em um arquivo JSON, você deve usar LudusLog.export();

          log.export(); // Por padrão, o log será exportado na pasta raiz do projeto/Resources.


      }

};

````
<h3>Output do arquivo JSON:</h3>

```json
{
  "title":"Meu primeiro log",
  "description":"Minha descrição"
  "reports":[
  {
    "id":"1",
    "title":"",
    "description":"",
    "date":"23/02/2024 23:44:02"
  },
  {
    "id":"2",
    "title":"Posição do mouse",
    "description":"POS(1,10,0)",
    "date":"23/02/2024 23:44:04"
  },
  ]
}

````

</p>

<h1>Use com responsabilidade e se tiver dúvidas, entre em contato.</h1>
<h3>Sempre estaremos de portas abertas para te ajudar no que for preciso. </h3>
<p>A documentação <strong>poderá sofrer mudanças</strong> com o avanço do projeto, então sempre <strong>acompanhe as versões mais atualizadas.</strong></p>

using UnityEngine;
using UnityEngine.SceneManagement;  // Certifique-se de que o namespace correto está incluído

public class SceneManagementScript : MonoBehaviour
{
    // Função para carregar uma cena pelo nome
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);  // Aqui está a classe SceneManager correta da Unity
    }

    // Função para carregar uma cena pelo índice no Build Settings
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);  // Aqui está a classe SceneManager correta da Unity
    }

    // Função para recarregar a cena atual
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();  // Obter a cena ativa atual
        SceneManager.LoadScene(currentScene.name);  // Recarregar a cena atual
    }

    // Função para sair do jogo (funciona apenas em builds, não no editor)
    public void QuitGame()
    {
        Debug.Log("Saindo do Jogo...");  // Mensagem de depuração
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;  // Certifique-se de que o namespace correto est� inclu�do

public class SceneManagementScript : MonoBehaviour
{
    // Fun��o para carregar uma cena pelo nome
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);  // Aqui est� a classe SceneManager correta da Unity
    }

    // Fun��o para carregar uma cena pelo �ndice no Build Settings
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);  // Aqui est� a classe SceneManager correta da Unity
    }

    // Fun��o para recarregar a cena atual
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();  // Obter a cena ativa atual
        SceneManager.LoadScene(currentScene.name);  // Recarregar a cena atual
    }

    // Fun��o para sair do jogo (funciona apenas em builds, n�o no editor)
    public void QuitGame()
    {
        Debug.Log("Saindo do Jogo...");  // Mensagem de depura��o
        Application.Quit();
    }
}

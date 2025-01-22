using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI gasText;
    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject gasPrefab;
    
    private int leftGas;
    public bool isGameActive = false;
    private int[] spawnXPoints = { -2, 0, 2 };

    IEnumerator UpdateGas()
    {
        while (leftGas > 0 && isGameActive)
        {
            yield return new WaitForSeconds(1f);
            leftGas -= 10;
            gasText.text = leftGas.ToString();
            if (leftGas <= 0)
            {
                GameOver();
            }
        }
    }

    public void GetGas()
    {
        leftGas += 30;
        gasText.text = leftGas.ToString();
    }

    IEnumerator SpawnGas()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(2f);
            var spawnXIndex = Random.Range(0, spawnXPoints.Length);
            Vector3 spawnPosition = new Vector3(spawnXPoints[spawnXIndex], 0.21f, 2);
            Instantiate(gasPrefab,spawnPosition,gasPrefab.transform.rotation);
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        titleScreen.SetActive(false);
        leftGas = 100;
        gasText.text = leftGas.ToString();
        
        StartCoroutine(UpdateGas());
        StartCoroutine(SpawnGas());
    }

    private void GameOver()
    {
        isGameActive = false;
        gameOverScreen.SetActive(true);
    }

    public void LoadTitleScreen()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

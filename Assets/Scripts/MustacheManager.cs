using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MustacheManager : MonoBehaviour
{
    private GameObject mustacheParent;  // O objeto com a tag Mustache
    private GameObject mustache1;       // Mustache 1
    private GameObject mustache2;       // Mustache 2
    private int currentMustache = 0;    // Controle para alternar entre os bigodes


    public void OnChangeMustacheButtonClicked()
    {
        // Encontrar o objeto com a tag "Mustache"
        mustacheParent = GameObject.FindWithTag("Mustache");

        // Verificar se o objeto foi encontrado
        if (mustacheParent != null)
        {
            // Procurar os filhos pelo nome
            mustache1 = mustacheParent.transform.Find("Mustache 1").gameObject;
            mustache2 = mustacheParent.transform.Find("Mustache 2").gameObject;

            // Inicialmente ocultar ambos os bigodes
            SetMustache(0);
        }
        else
        {
            Debug.LogError("Nenhum objeto com a tag 'Mustache' foi encontrado!");
        }

        // Alternar entre 0 (nenhum), 1 (mustache1), e 2 (mustache2)
        currentMustache = (currentMustache + 1) % 3;
        SetMustache(currentMustache);
    }

    void SetMustache(int index)
    {
        if (mustache1 != null && mustache2 != null)
        {
            // Nenhum bigode visível
            if (index == 0)
            {
                mustache1.SetActive(false);
                mustache2.SetActive(false);
            }
            // Mustache 1 visível
            else if (index == 1)
            {
                mustache1.SetActive(true);
                mustache2.SetActive(false);
            }
            // Mustache 2 visível
            else if (index == 2)
            {
                mustache1.SetActive(false);
                mustache2.SetActive(true);
            }
        }
    }
}
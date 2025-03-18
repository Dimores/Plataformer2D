using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orby
{
    public class HandIK : MonoBehaviour
    {
        public Transform pontoSeguranca; // Referência ao ponto na arma

        void LateUpdate()
        {
            if (pontoSeguranca != null)
            {
                transform.rotation = pontoSeguranca.rotation; // Faz a mão girar junto com a arma
            }
        }

    }
}

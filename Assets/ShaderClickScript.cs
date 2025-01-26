using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderClickScript : MonoBehaviour
{
    [SerializeField]Material mat;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            mat.SetVector("_ClickPos", hit.point);
            mat.SetFloat("_SplashTime", Time.time);
            mat.SetColor("_ColorA", mat.GetColor("_ColorB"));
            mat.SetColor("_ColorB", new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value,1));
        }
    }
}

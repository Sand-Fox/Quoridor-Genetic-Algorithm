using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AIWeightUI : MonoBehaviour
{
    [SerializeField] private IASide _IASide;

    [SerializeField] private TMP_InputField _paramFieldX;
    [SerializeField] private TMP_InputField _paramFieldY;
    [SerializeField] private TMP_InputField _paramFieldZ;
    [SerializeField] private TMP_InputField _paramFieldW;

    private float _paramX = 0.5f;
    private float _paramY = 0.5f;
    private float _paramZ = 0.5f;
    private float _paramW = 0.5f;

    public void OnParamXChange(string text)
    {
        if (float.TryParse(text, out float result)) _paramX = result;
        else _paramFieldX.text = _paramX.ToString();
    }

    public void OnParamYChange(string text)
    {
        if (float.TryParse(text, out float result)) _paramY = result;
        else _paramFieldY.text = _paramY.ToString();
    }

    public void OnParamZChange(string text)
    {
        if (float.TryParse(text, out float result)) _paramZ = result;
        else _paramFieldZ.text = _paramZ.ToString();
    }

    public void OnParamWChange(string text)
    {
        if (float.TryParse(text, out float result)) _paramW = result;
        else _paramFieldW.text = _paramW.ToString();
    }

    public void SetWeightOnPlay()
    {
        if (_IASide == IASide.IABot) SceneSetUpManager.IAWeightBot = new Vector4(_paramX, _paramY, _paramZ, _paramW);
        if (_IASide == IASide.IATop) SceneSetUpManager.IAWeightTop = new Vector4(_paramX, _paramY, _paramZ, _paramW);
    }

    public enum IASide
    {
        IABot,
        IATop
    }
}

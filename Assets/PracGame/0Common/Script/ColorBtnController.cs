using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ColorBtnController : MonoBehaviour
{
    public Image setColorImg;

    public Image[] colorTextImg;
    public TextMeshProUGUI[] colorIndexText;
    public Button[] colorBtn;
    public Button[] colorMinusBtn;
    public int[] colorCoefficientIndex;
    public int sumIndex;
    
    void Start()
    {
        SetColorBtn();
    }

    public void ColorBtn(int pIndex)
    {
        colorCoefficientIndex[pIndex]++;
        colorMinusBtn[pIndex].gameObject.SetActive(true);
        sumIndex++;
        ResultColor();
    }

    public void ColorMinusBtn(int pIndex)
    {
        
        colorCoefficientIndex[pIndex]--;
        if (colorCoefficientIndex[pIndex] == 0) colorMinusBtn[pIndex].gameObject.SetActive(false);
        sumIndex--;
        ResultColor();
    }

    public void ResultColor()
    {
        if (sumIndex == 0)
        {
            setColorImg.color = Color.white;
        }
        else
        {
            setColorImg.color = (Color.yellow * colorCoefficientIndex[0] +
                                Color.red * colorCoefficientIndex[1] +
                                Color.magenta * colorCoefficientIndex[2] +
                                Color.blue * colorCoefficientIndex[3] +
                                Color.cyan * colorCoefficientIndex[4] +
                                Color.green * colorCoefficientIndex[5] +
                                Color.white * colorCoefficientIndex[6] +
                                Color.black * colorCoefficientIndex[7]) 
                                / sumIndex;
        }

        SetColorText();
    }

    public void SetColorText()
    {
        for (int i = 0; i < colorTextImg.Length; i++)
        {
            colorIndexText[i].text = Mathf.RoundToInt( (colorCoefficientIndex[i] / (float)sumIndex) * 100).ToString() + "%";
        }
    }
    public void SetColorBtn()
    {
        for (int i = 0; i < colorTextImg.Length; i++)
        {
            colorIndexText[i] = colorTextImg[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
        colorTextImg[0].GetComponent<Image>().color = Color.yellow;
        colorTextImg[1].GetComponent<Image>().color = Color.red;
        colorTextImg[2].GetComponent<Image>().color = Color.magenta;
        colorTextImg[3].GetComponent<Image>().color = Color.blue;
        colorTextImg[4].GetComponent<Image>().color = Color.cyan;
        colorTextImg[5].GetComponent<Image>().color = Color.green;
        colorTextImg[6].GetComponent<Image>().color = Color.white;
        colorTextImg[7].GetComponent<Image>().color = Color.black;

        colorBtn[0].GetComponent<Image>().color = Color.yellow;
        colorBtn[1].GetComponent<Image>().color = Color.red;
        colorBtn[2].GetComponent<Image>().color = Color.magenta;
        colorBtn[3].GetComponent<Image>().color = Color.blue;
        colorBtn[4].GetComponent<Image>().color = Color.cyan;
        colorBtn[5].GetComponent<Image>().color = Color.green;
        colorBtn[6].GetComponent<Image>().color = Color.white;
        colorBtn[7].GetComponent<Image>().color = Color.black;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PTEUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    RectTransform rect = null;
    [SerializeField] Button button = null;
    [SerializeField] Text tNumber = null;
    [SerializeField] Text tSymbol = null;
    [SerializeField] Text tName = null;
    [SerializeField] Outline outLine = null;

    public static Vector2 originPt = Vector2.zero;
    public static float size = 150f;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        
    }
    public void Init(ChemicalElement e)
    {
        tNumber.text = e.Number.ToString();
        tSymbol.text = e.Symbol;
        tName.text = e.Name;
        gameObject.name = e.Number.ToString() + "-" + e.Name;
        button.image.color = PeriodTable.Instance.data.Colors[(int)e.chemicalProperty];
        if((e.chemicalProperty == ChemicalProperty.Actinide || e.chemicalProperty == ChemicalProperty.Lanthanide) && e.Column != 3)
        {
            rect.anchoredPosition = new Vector3(originPt.x + size * e.Column, originPt.y - size * (e.Row + 3), 0);
        }
        else
        {
            rect.anchoredPosition = new Vector3(originPt.x + size * e.Column, originPt.y - size * e.Row, 0);
        }
        button.onClick.AddListener(() => { PeriodTable.Instance.ElementClick(e); });
    }

    public void OnSelect(BaseEventData eventData)
    {
        outLine.enabled = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        outLine.enabled = false;
    }
}

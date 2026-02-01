using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PeriodTableData", menuName ="Level10/PeriodTable")]
public class PTData : ScriptableObject
{
    public List<ChemicalElement> ElementData;
    public Color[] Colors;
}

[Serializable]
public class ChemicalElement
{
    public int Number;
    public int Row;
    public int Column;
    public float Mass;
    public string Symbol;
    public string Name;
    public string Detail;
    public ChemicalProperty chemicalProperty;
}
public enum ChemicalProperty
{
    AlkaliMetal,
    AlkalineEarthMetal,
    TransitionMetal,
    PostTransitionMetal,
    Metalloid,
    ReactiveNonmetal,
    NobleGas,
    Lanthanide,
    Actinide,
    UnknownChemicallProperties
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingRecipe
{
    public string ItemName;

    public string Req1;
    public string Req2;

    public int Req1Amount;
    public int Req2Amount;

    public int numOfReqs;

    public CraftingRecipe(string name, int reqNum, string R1, int r1Num, string R2, int r2Num)
    {
        ItemName = name;

        numOfReqs = reqNum;//kac adet degiskene ihtiyacimiz var

        Req1 = R1;
        Req2 = R2;

        Req1Amount = r1Num;
        Req2Amount = r2Num;
    }

}

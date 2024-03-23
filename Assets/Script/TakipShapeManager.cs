using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakipShapeManager : MonoBehaviour
{
    ShapeManager takipShape = null;

    bool dibeDegdimi = false;

    public Color color = new Color(1f, 1f, 1f, 0.2f);

    public void TakipShapeOlustur(ShapeManager gercekShape, BoardManager board)
    {
        if (!takipShape)
        {
            takipShape = Instantiate(gercekShape, gercekShape.transform.position, gercekShape.transform.rotation) as ShapeManager;

            takipShape.name = "TakipShape";

            SpriteRenderer[] tumSprite = takipShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sr in tumSprite)
            {
                sr.color = color;
            }
        }
        else
        {
            takipShape.transform.position = gercekShape.transform.position;
            takipShape.transform.rotation = gercekShape.transform.rotation;
        }

        dibeDegdimi = false;

        while (!dibeDegdimi)
        {
            takipShape.AsagiHareket();
            if (!board.GecerliPozisyondami(takipShape))
            {
                takipShape.YukariHareket();

                dibeDegdimi = true;
            }
        }
    }

    public void Reset()
    {
        Destroy(takipShape.gameObject);
    }
}

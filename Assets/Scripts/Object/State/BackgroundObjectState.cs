using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundObjectState : BaseObjectState
{
    // ========== Public methods ==========
    public override void TransformState(bool isMainState)
    {
        int sortingOrder = (int)StateSortingLayerOrder.VISIBLE_BACK_GROUND;
        SpriteMaskInteraction maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

        if (isMainState)
        {
            sortingOrder = (int)StateSortingLayerOrder.VISIBLE_BACK_GROUND;
            maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        else
        {
            sortingOrder = (int)StateSortingLayerOrder.HIDDEN_BACK_GROUND;
            maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }

        ChangeSortingOrder(sortingOrder);
        ChangeMaskInteraction(maskInteraction);
    }
}

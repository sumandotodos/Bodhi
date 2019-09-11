using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public ListController listController;
    public Transform slabsParent;
    public Transform overParent;

    Transform slabTransform;
    Vector2 TouchPoint;
    Vector2 OriginalPosition;

    int StartIndex;
    int EndIndex;

    System.Action TouchUpdate;

    float Factor = 3.8f;

    float EffectiveHeight;

    int CurrentPositionInList;

    private void Awake()
    {
        TouchUpdate = NotTouching;
        Factor = (1920.0f / ((float)Screen.height));
    }

    private void Update()
    {
        TouchUpdate();
    }

    public void BeginTouch(int index)
    {
        StartIndex = index;
        Slab touchedSlab = listController.GetSlab(index);
        touchedSlab.GetComponent<UIGeneralFader>().SetOpacity(0.35f);
        slabTransform = touchedSlab.GetComponent<Transform>();
        slabTransform.SetParent(overParent);
        TouchPoint = Input.mousePosition;
        OriginalPosition = slabTransform.GetComponent<Magnetor>().Destination;
        slabTransform.GetComponent<Magnetor>().Going = false;
        float SlabHeight = touchedSlab.GetHeight();
        EffectiveHeight = touchedSlab.Adjust(SlabHeight);//(SlabHeight + 15.0f + SlabHeight / 6.0f);
        TouchUpdate = Touching;
        CurrentPositionInList = index;
    }

    public void EndTouch(int index)
    {
        slabTransform.SetParent(slabsParent);
        slabTransform.GetComponent<UIGeneralFader>().SetOpacity(1.0f);
        slabTransform.GetComponent<Magnetor>().CurrentPosition = slabTransform.localPosition;
        //slabTransform.GetComponent<Magnetor>().Destination = OriginalPosition;
        slabTransform.GetComponent<Magnetor>().Going = true;
        EndIndex = CalculatePositionInList(slabTransform.GetComponent<Magnetor>().Destination.y);
        //Debug.Log("<color=green>Exchange " + StartIndex + " to " + EndIndex + "</color>");
        ItemsController.GetSingleton().NotifyDragEnd(StartIndex, EndIndex);
        TouchUpdate = NotTouching;
        listController.listIds();

        TypeOfContent contentFilter;

        contentFilter = Heart.FavTypeFromString(PlayerPrefs.GetString("FavoriteType"));

        if(contentFilter == TypeOfContent.Question)
        {
            string favId = listController.GetSlab(0).id;
            API.GetSingleton().PutFavoriteQuestion(PlayerPrefs.GetString("UserId"), favId,
                (err, text) =>
                {

                });
        }

    }

    public void NotTouching()
    {
        // nothing here
    }

    public void Touching()
    {
        Vector2 CurrentPosition = Input.mousePosition;
        Vector2 Displacement = CurrentPosition - TouchPoint;
        slabTransform.localPosition = OriginalPosition + Factor * Displacement;
        int PositionInList = CalculatePositionInList(slabTransform.localPosition.y);
        if(PositionInList != CurrentPositionInList)
        {
            int Displacements = PositionInList - CurrentPositionInList;
            if(Displacements > 0)
            {
                for(int i = CurrentPositionInList; i < PositionInList; ++i)
                {
                    listController.GetSlab(i+1).GetComponent<Magnetor>().DisplaceTargetPosition(new Vector2(0, EffectiveHeight));
                    float movedSlabHeight = listController.GetSlab(i + 1).GetEffectiveHeight();
                    slabTransform.GetComponent<Magnetor>().DisplaceTargetPosition(new Vector2(0, -movedSlabHeight));
                }
                for(int i = CurrentPositionInList; i < PositionInList; ++i)
                {
                    listController.SwapSlabs(i, i + 1);
                }
            }
            else if (Displacements < 0)
            {
                for (int i = CurrentPositionInList; i > PositionInList; --i)
                {
                    listController.GetSlab(i-1).GetComponent<Magnetor>().DisplaceTargetPosition(new Vector2(0, -EffectiveHeight));
                    float movedSlabHeight = listController.GetSlab(i - 1).GetEffectiveHeight();
                    slabTransform.GetComponent<Magnetor>().DisplaceTargetPosition(new Vector2(0, movedSlabHeight));
                }
                for (int i = CurrentPositionInList; i > PositionInList; i--)
                {
                    listController.SwapSlabs(i, i - 1);
                }
            }
            CurrentPositionInList = PositionInList;

        }
    }

    public int CalculatePositionInList(float YCoord)
    {
        for(int i = 0; i < listController.GetNumberOfSlabs()-1; ++i)
        {
            if(YCoord > (listController.GetSlab(i+1).GetComponent<Magnetor>().Destination.y+50.0f))
            {
                //Debug.Log("<color=purple>Pos in list: " + i + "</color>");
                return i;
            }
        }
        return listController.GetNumberOfSlabs() - 1;
    }
}

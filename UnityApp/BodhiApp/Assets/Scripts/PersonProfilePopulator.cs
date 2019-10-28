using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonProfilePopulator : ItemPopulator
{
    public Texture2D DefaultUserAvatar;
    public AvatarTaker avatarTaker;
    public GameObject HeaderPrefab;
    public GameObject ContributionPrefab;
    public GameObject PersonsFavoritePrefab;
    public GameObject FollowSlab;
    public GameObject EmptySlab;
    public GameObject AvatarSlab;
    public GameObject CommunicationsSlab;

    public ContentsManager contentsManager;

    public string ProfileUserId;
    public bool FollowingThisUser = false;
    public int CommStateForThisUser = -1;
    public string ThisUserPhone = "";
    public Texture2D ThisUserAvatar = null;

    int CommonMeans;

    override public Coroutine GetItems(System.Action<List<ListItem>> callback)
    {

        return StartCoroutine(PopulateProfileContribsAndFavoritesCoroutine(callback));

    }

    IEnumerator PopulateProfileContribsAndFavoritesCoroutine(System.Action<List<ListItem>> Callback)
    {
        List<ListItem> listItems = new List<ListItem>();
        string OtherUserHandle = PlayerPrefs.GetString("OtherUserHandle");
        string OtherUserId = PlayerPrefs.GetString("OtherUserId");

        if(OtherUserHandle == "")
        {
            yield return API.GetSingleton().GetHandle(OtherUserId, (err, handle) =>
            {
                OtherUserHandle = handle;
            });
        }

        Debug.Log("OtherUserId = " + OtherUserId);
        Debug.Log("OtherUserHandle = " + OtherUserHandle);
        Debug.Log("UserId = " + PlayerPrefs.GetString("UserId"));

        ProfileUserId = OtherUserId;

        bool following = false;

        yield return API.GetSingleton().GetAvatar(ProfileUserId, (res, ok, tex) =>
        {
            Debug.Log("User profile avatar get with status: " + ok);
            ThisUserAvatar = avatarTaker.ApplyMaskTexture(ok ? tex : DefaultUserAvatar);
        });

       yield return API.GetSingleton().IsFollowing(PlayerPrefs.GetString("UserId"), OtherUserId, (err, result) =>
        {
            if(err==null)
            {
                following = result;
            }
        });

        FollowingThisUser = following;

        // Follow / Unfollow section
        listItems.Add(new ListItem(OtherUserId, Color.grey, "", "", "", "", FollowSlab));

        int MeToThisUser = -1;
        int ThisUserToMe = -1;

        yield return API.GetSingleton().GetCommsPreference(PlayerPrefs.GetString("UserId"), OtherUserId,
        (err, value) =>
        {
            MeToThisUser = value;
            CommStateForThisUser = value;
        });

        yield return API.GetSingleton().GetCommsPreference(OtherUserId, PlayerPrefs.GetString("UserId"),
        (err, value) =>
        {
            ThisUserToMe = value;
        });

        CommonMeans = GetCommonMeans(MeToThisUser, ThisUserToMe);

        // Communication preference panel
        listItems.Add(new ListItem(OtherUserId, Color.white, "", "", "", "", CommunicationsSlab));

        // Profile header section ...
        listItems.Add(new ListItem("", Color.grey, "Perfil de " + OtherUserHandle, "", "", "", HeaderPrefab));

        // User's avatar
        listItems.Add(new ListItem("", Color.white, "", "", "", "", AvatarSlab));

        // ... and profile section
        yield return API.GetSingleton().GetProfile(OtherUserId, (err, profile) =>
        {
            if (profile.about == "")
            {
                listItems.Add(new ListItem("", Color.gray, "Sin perfil", "", "", "", EmptySlab));
            }
            else
            {
                listItems.Add(new ListItem(OtherUserId, Color.cyan, profile.about, "", "", "", SlabPrefab));
                ThisUserPhone = profile.phone;
            }
        });

        // Contributions header ...
        listItems.Add(new ListItem("", Color.grey, "Contribuciones de " + OtherUserHandle, "", "", "", HeaderPrefab));
        // ... and contributions
        yield return API.GetSingleton().GetContributionsList(OtherUserId, (err, result) =>
        {
            if (result.result.Count == 0)
            {
                listItems.Add(new ListItem("", Color.gray, "No hay contribuciones", "", "", "", EmptySlab));
            }
            else
            {
                for (int i = 0; i < result.result.Count; ++i)
                {
                    Color col = ColorByCategory.GetSingleton().ResolveColor(result.result[i]._id);
                    if (!result.result[i].validated)
                    {
                        col = Color.gray;
                    }
                    listItems.Add(new ListItem(result.result[i]._id, col, result.result[i].content, "", "", "", ContributionPrefab));
                }
            }
        });

        // Favorites headers ...
        listItems.Add(new ListItem("", Color.grey, "Favoritos de " + OtherUserHandle, "", "", "", HeaderPrefab));
        // ... and favorites
        List<string> idsToLoad = null;
        yield return API.GetSingleton().GetFavoritesList(OtherUserId, (err, result) =>
        {
            idsToLoad = result.favorites;
        });
        if (idsToLoad.Count == 0)
        {

            listItems.Add(new ListItem("", Color.gray, "Ningún favorito", "", "", "", EmptySlab));
        }
        else
        {
            foreach (string id in idsToLoad)
            {
                Color col = ColorByCategory.GetSingleton().ResolveColor(id);
                string content = "";
                if (ContentsManager.IsLocalContent(id))
                {
                    content = contentsManager.GetLocalContentFromId(id);
                }
                else
                {
                    yield return API.GetSingleton().GetItemContent(id, (err, cont) =>
                     {
                         content = cont;
                     });
                }
                listItems.Add(new ListItem(id, col, content, "", content, OtherUserId, PersonsFavoritePrefab));

            }
        }

        // With all items ready, callback(listItems)
        Callback(listItems);
    }

    public override void PostInstancing()
    {
        FollowSlab fSlab = FindObjectOfType<FollowSlab>();
        if (fSlab != null)
        {
            fSlab.SetFollowState(FollowingThisUser ? FollowState.Following : FollowState.NotFollowing);
        }
        CommsSlab cSlab = FindObjectOfType<CommsSlab>();
        if(cSlab != null)
        {
            cSlab.SetIndex(CommStateForThisUser);
            cSlab.OtherUserId = ProfileUserId;
            cSlab.SetAgreement(IndexToMeansName(CommonMeans));
            cSlab.SetPhoneNumber(ThisUserPhone);
            StartCommsCoincidenceService(cSlab, ProfileUserId, 5.0f);
        }
        ImageSlab iSlab = FindObjectOfType<ImageSlab>();
        if(iSlab != null)
        {
            Debug.Log("Setting image slab image");
            iSlab.SetImage(ThisUserAvatar);
        }
        else
        {
            Debug.Log("Could not find a slab image");
        }
    }

    override public void DeleteItemCallback(string id)
    {
       
    }

    public void TouchOnAnswer(int index)
    {

    }

    private int GetCommonMeans(int means1, int means2)
    {
        return Mathf.Min(means1, means2);
    }

    private string IndexToMeansName(int index)
    {
        switch(index)
        {
            case -1:
                return "";
            case 0:
                return "Chat";
            case 1:
                return "Audio";
            case 2:
                return "Vídeo";
            default:
                return "";
        }
    }

    Coroutine CommsCoincidenceServiceHandle;

    public void StartCommsCoincidenceService(CommsSlab slab, string OtherUserId, float Interval)
    {
        CommsCoincidenceServiceHandle = StartCoroutine(CommsCoincidenceService(slab, OtherUserId, Interval));
    }

    public void StopCommsCoincidenceService()
    {
        StopCoroutine(CommsCoincidenceServiceHandle);
    }

    IEnumerator CommsCoincidenceService(CommsSlab slab, string OtherUserId, float Interval)
    {
        int MeToThisUser = -1;
        int ThisUserToMe = -1;
        int _CommonMeans = -1;
        while(1<2)
        {
            yield return API.GetSingleton().GetCommsPreference(PlayerPrefs.GetString("UserId"), OtherUserId,
            (err, value) =>
            {
                MeToThisUser = value;
                CommStateForThisUser = value;
            });

            yield return API.GetSingleton().GetCommsPreference(OtherUserId, PlayerPrefs.GetString("UserId"),
            (err, value) =>
            {
                ThisUserToMe = value;
            });

            _CommonMeans = GetCommonMeans(MeToThisUser, ThisUserToMe);

            slab.SetAgreement(IndexToMeansName(_CommonMeans));

            Debug.Log("<color=gree>Comms refresh!</color>");

            yield return new WaitForSeconds(Interval);
        }
    }
}

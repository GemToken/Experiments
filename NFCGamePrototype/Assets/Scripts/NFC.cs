using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class NFC : MonoBehaviour {

	public string tagID;
    private string payloadText;

	public Text tagOutputText;
    public Text lastScannedText;

    public GameObject figureParent;
    public GameObject currentFig;
	public bool tagFound = false;
    private bool nfcIsOn = true;
    public Button toHubButton;
    public Button playLastButton;

	private AndroidJavaObject mActivity;
	private AndroidJavaObject mIntent;
	private string sAction;

    AndroidJavaObject currentActivity;
    AndroidJavaClass UnityPlayer;
    AndroidJavaObject context;
    AndroidJavaObject manager;
    AndroidJavaObject adapter;

    public PlayerManager playermanager;

    void Start() {

        if (Application.platform == RuntimePlatform.Android)
        {
            UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
            showLastPlayButton();
        }
    }

	void Update() {
		if (Application.platform == RuntimePlatform.Android) {
            
            if (!nfcEnabled(context) && nfcIsOn)
            {
                tagOutputText.text = "Warning: NFC is off!";
                tagOutputText.color = Color.red;
                nfcIsOn = false;
            } else if (nfcEnabled(context) && !nfcIsOn)
            {
                tagOutputText.text = "Tap an NFC figure to start playing.";
                tagOutputText.color = Color.white;
                nfcIsOn = true;
            } 

            try
            {
                // Create new NFC Android object
                mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"); // Activities open apps
                mIntent = mActivity.Call<AndroidJavaObject>("getIntent");
                sAction = mIntent.Call<String>("getAction");

                if (sAction == "android.nfc.action.NDEF_DISCOVERED")
                {
                    sAction = null;
                    Debug.Log("Tag of type NDEF");
                    AndroidJavaObject[] mNdefMessage = mIntent.Call<AndroidJavaObject[]>("getParcelableArrayExtra", "android.nfc.extra.NDEF_MESSAGES");
                    if (mNdefMessage != null)
                    {
                        // Handle NFC payload
                        AndroidJavaObject[] mNdefRecord = mNdefMessage[0].Call<AndroidJavaObject[]>("getRecords");
                        byte[] payLoad = mNdefRecord[0].Call<byte[]>("getPayload");
                        byte[] adjustedPayload = new byte[payLoad.Length - 3];

                        // Necessary adjustment to remove the language bytes from text. May need changing in the future.
                        Array.Copy(payLoad, 3, adjustedPayload, 0, adjustedPayload.Length);
                        payloadText = System.Text.Encoding.UTF8.GetString(adjustedPayload);
                        //tagOutputText.text = "You Scanned: " + text;
                        tagID = payloadText;
                        mIntent.Call("removeExtra", "android.nfc.extra.TAG");

                        loadFigure(payloadText);
                        
                        mNdefMessage = null;
                    }
                    else
                    {
                        if (!tagFound)
                        {
                            tagOutputText.text = "No data on tag.";
                        }
                    }
                    tagFound = true;

                    // Two lines below allow for multiple tag scanning
                    mIntent.Call("removeExtra", "android.nfc.extra.TAG");
                    mIntent.Call("removeExtra", "android.nfc.extra.NDEF_MESSAGES");
                    return;
                }
                else if (sAction == "android.nfc.action.TECH_DISCOVERED")
                {
                    //duplicate code above if this field becomes necessary
                    return;
                }
                else if (sAction == "android.nfc.action.TAG_DISCOVERED")
                {
                    sAction = null;
                    Debug.Log("This type of tag is not supported !");
                }
                else
                {
                    sAction = null;
                    //tag_output_text.text = "...Awaiting Scan...";
                    return;
                }
                sAction = null;
            }
            catch (Exception ex)
            {
                string text = ex.Message;
                print(ex.Message);
                tagOutputText.text = "Error occured during NFC Scan";
            }
		}
	}

    void showToast(String toastString)
    {
        Debug.Log(this + ": Running on UI thread");

        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
        toast.Call("show");
    }

    public static string getLastTappedFigure()
    {
        string lastTap = PlayerPrefs.GetString("lastTap");
        if (lastTap == "")
        {
            lastTap = "None";
        }
        return lastTap;
    }

    public void showLastPlayButton()
    {
        if (getLastTappedFigure() != "None")
        {
            playLastButton.gameObject.SetActive(true);
            lastScannedText.text = "Last Tapped: " + getLastTappedFigure();
        }
    }

    public void loadFigure(string name)
    {
        //Hide the currentFig on screen if shown
        if (currentFig != null)
        {
            currentFig.SetActive(false);
        }

        // Update onscreen figure to the new scanned figure
        currentFig = figureParent.transform.Find(name).gameObject;
        currentFig.SetActive(true);
        toHubButton.interactable = true;
        PlayerPrefs.SetString("lastTap", name);
        playermanager.setupPlayerData(name);
        playermanager.updateStatText();

        // Vibration
        Handheld.Vibrate();

        //Hide the playLast button
        playLastButton.gameObject.SetActive(false);
    }

    public void loadLastFigure()
    {
        loadFigure(getLastTappedFigure());
    }


    bool nfcEnabled(AndroidJavaObject context)
    {
        try
        {
            manager = context.Call<AndroidJavaObject>("getSystemService", "nfc");
            adapter = manager.Call<AndroidJavaObject>("getDefaultAdapter");
            if (adapter.Call<bool>("isEnabled") == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return false;
        }
    }

}

public static class NFCTools
{
    public static AndroidJavaObject ClassForName(string className)
    {
        using (var clazz = new AndroidJavaClass("java.lang.Class"))
        {
            return clazz.CallStatic<AndroidJavaObject>("forName", className);
        }
    }

    // Cast extension method
    public static AndroidJavaObject Cast(this AndroidJavaObject source, string destClass)
    {
        using (var destClassAJC = ClassForName(destClass))
        {
            return destClassAJC.Call<AndroidJavaObject>("cast", source);
        }
    }

    // Cast extension method
    public static AndroidJavaObject Cast(this AndroidJavaClass source, string destClass)
    {
        using (var destClassAJC = ClassForName(destClass))
        {
            return destClassAJC.Call<AndroidJavaObject>("cast", source);
        }
    }

    // Get system service which has been cased to serviceclass
    public static AndroidJavaObject GetSystemService(AndroidJavaObject context, string name, string serviceClass)
    {
        try
        {
            var serviceObj = context.Call<AndroidJavaObject>("getSystemService", name);
            return serviceObj.Cast(serviceClass);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to get " + name + " service. Error: " + e.Message);
            return null;
        }
    }

    public static AndroidJavaObject GetDefaultAdapter(AndroidJavaObject context, string serviceClass)
    {
        try
        {
            var serviceObj = context.Call<AndroidJavaObject>("getDefaultAdapter");
            return serviceObj.Cast(serviceClass);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed to get default NFCAdapter. Error: " + e.Message);
            return null;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorPicker : MonoBehaviour, IPointerClickHandler
{
    // magSects -> angSects -> Color
    private Dictionary<int, Dictionary<int, string>> pickerMap;


    int HexToDec(string hex) {
        int val = 0;
        foreach (char c in hex) {
            val *= 16;
            if ((c - 'a' >= 0) && (c - 'a') < 6) {
                val += (c - 'a') + 10;
            } else if ((c - 'A' >= 0) && (c - 'A' < 6)) {
                val += (c - 'A') + 10;
            } else {
                val += (c - '0');
            }
        }
        return val;
    }

    Color HexToColor(string hex) {
        int a = 255;
        if (hex.Length == 8) {
            a = HexToDec(hex.Substring(6, 2));
        }
        int r = HexToDec(hex.Substring(0, 2));
        int g = HexToDec(hex.Substring(2, 2));
        int b = HexToDec(hex.Substring(4, 2));
        return new Color32(
            Convert.ToByte(r),
            Convert.ToByte(g),
            Convert.ToByte(b),
            Convert.ToByte(a));
    }

    // Start is called before the first frame update
    void Start()
    {
        pickerMap = new Dictionary<int, Dictionary<int, string>>(); 
        
        Dictionary<int, string> mag1 = new Dictionary<int, string>();
        mag1[1] = "b57caaff";
        mag1[2] = "d27fb8ff";
        mag1[3] = "eb8aa7ff";
        mag1[4] = "eb8a82ff";
        mag1[5] = "e2a877ff";
        mag1[6] = "f0aa69ff";
        mag1[7] = "f5cd74ff";
        mag1[8] = "fcde6cff";
        mag1[9] = "c7de83ff";
        mag1[10]= "bbc8caff";
        mag1[11]= "85aee9ff";
        mag1[12]= "7f9ad3ff";
        pickerMap[1] = mag1;
        
        Dictionary<int, string> mag2 = new Dictionary<int, string>();
        mag2[1] = "a2558eff";
        mag2[2] = "ce67abff";
        mag2[3] = "e66c99ff";
        mag2[4] = "ea5d60ff";
        mag2[5] = "e78a56ff";
        mag2[6] = "f19741ff";
        mag2[7] = "f5c038ff";
        mag2[8] = "f9db49ff";
        mag2[9] = "acd858ff";
        mag2[10]= "74b0b2ff";
        mag2[11]= "5196e1ff";
        mag2[12]= "4a69bdff";
        pickerMap[2] = mag2;
        
        Dictionary<int, string> mag3 = new Dictionary<int, string>();
        mag3[1] = "8e4577ff";
        mag3[2] = "b34190ff";
        mag3[3] = "e22765ff";
        mag3[4] = "e82e2bff";
        mag3[5] = "d96928ff";
        mag3[6] = "ec7416ff";
        mag3[7] = "f3b50eff";
        mag3[8] = "f7d20bff";
        mag3[9] = "93bb41ff";
        mag3[10]= "519c9cff";
        mag3[11]= "267bcfff";
        mag3[12]= "1e3799ff";
        pickerMap[3] = mag3;
        
        Dictionary<int, string> mag4 = new Dictionary<int, string>();
        mag4[1] = "753d68ff";
        mag4[2] = "913b7aff";
        mag4[3] = "af1c41ff";
        mag4[4] = "b71e1aff";
        mag4[5] = "af5727ff";
        mag4[6] = "c95712ff";
        mag4[7] = "d48f0cff";
        mag4[8] = "d8bb08ff";
        mag4[9] = "77952cff";
        mag4[10]= "478983ff";
        mag4[11]= "205d91ff";
        mag4[12]= "0c2461ff";
        pickerMap[4] = mag4;
    }

    public void OnPointerClick(PointerEventData ped) {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), ped.position, ped.pressEventCamera, out localCursor))
            return;
        Vector2 scaledP = new Vector2(
            localCursor.x / GetComponent<RectTransform>().rect.width * 2,
            localCursor.y / GetComponent<RectTransform>().rect.height * 2);
        if (scaledP.magnitude >= 1.0f) {
            return;
        }

        int magSect = (int)(scaledP.magnitude/0.20f);
        float ang = Vector2.SignedAngle(scaledP, new Vector2(0, 1.0f));
        int angSect = 1 + (int)(ang/30.0f);
        if (ang < 0) {
            angSect = 12 + (int)(ang/30.0f);
        }

        Color newColor;
        if (magSect == 0) {
            if (ang > 0) {
                newColor = Color.black; 
            } else {
                newColor = Color.white; 
            }
        } else {
            newColor = HexToColor(pickerMap[magSect][angSect]);
        }

        foreach (IDable idable in GlobalVars.selected) {
            Renderer ballRend = idable.GetComponent<Renderer>();
            ballRend.material.color = newColor;
        }
        FrameData.UpdateBallsInFrame(FrameData.selectedFrame);
    }
}

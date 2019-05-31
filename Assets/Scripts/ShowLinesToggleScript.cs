using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowLinesToggleScript : MonoBehaviour {

    public Toggle toggle_;

	// Use this for initialization
	void Start () {
        toggle_.onValueChanged.AddListener(delegate {
            Toggled();
        });
	}
	
	// Update is called once per frame
	void Update () {
	}

    void Toggled() {
        Debug.Log("TOGGLED " + toggle_.isOn);
        bool be_active = toggle_.isOn;
        foreach (IDable line in GlobalVars.linesList) {
            line.gameObject.active = be_active;
        }
        foreach (Endpoint point in GlobalVars.pointsList) {
            point.gameObject.active = be_active;
        }
    }
}

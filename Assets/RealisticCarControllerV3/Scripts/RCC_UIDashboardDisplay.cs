//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2014 - 2020 BoneCracker Games
// http://www.bonecrackergames.com
// Buğra Özdoğanlar
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Handles RCC Canvas dashboard elements.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/RCC UI Dashboard Displayer")]
[RequireComponent (typeof(RCC_DashboardInputs))]
public class RCC_UIDashboardDisplay : MonoBehaviour {

	// Getting an Instance of Main Shared RCC Settings.
	#region RCC Settings Instance

	private RCC_Settings RCCSettingsInstance;
	private RCC_Settings RCCSettings {
		get {
			if (RCCSettingsInstance == null) {
				RCCSettingsInstance = RCC_Settings.Instance;
				return RCCSettingsInstance;
			}
			return RCCSettingsInstance;
		}
	}

	#endregion

	private RCC_DashboardInputs inputs;

	public DisplayType displayType;
	public enum DisplayType{Full, Customization, TopButtonsOnly, Off}

	public GameObject topButtons;
	public GameObject controllerButtons;
	public GameObject gauges;
	public GameObject customizationMenu;
	
	public Text KMHLabel;
	
	public Dropdown mobileControllers;

	void Awake(){

		inputs = GetComponent<RCC_DashboardInputs>();

		if (!inputs) {

			enabled = false;
			return;

		}

	}
	
	void Start () {
		
		CheckController ();
		
	}

	void OnEnable(){

		RCC_SceneManager.OnControllerChanged += CheckController;

	}

	private void CheckController(){

		if (RCCSettings.selectedControllerType == RCC_Settings.ControllerType.Keyboard || RCCSettings.selectedControllerType == RCC_Settings.ControllerType.XBox360One || RCCSettings.selectedControllerType == RCC_Settings.ControllerType.PS4 || RCCSettings.selectedControllerType == RCC_Settings.ControllerType.LogitechSteeringWheel) {

			if(mobileControllers)
				mobileControllers.interactable = false;
			
		}

		if (RCCSettings.selectedControllerType == RCC_Settings.ControllerType.Mobile) {

			if(mobileControllers)
				mobileControllers.interactable = true;
			
		}

	}

	void Update(){

		switch (displayType) {

		case DisplayType.Full:

			if(topButtons && !topButtons.activeInHierarchy)
				topButtons.SetActive(true);

			if(controllerButtons && !controllerButtons.activeInHierarchy)
				controllerButtons.SetActive(true);

			if(gauges && !gauges.activeInHierarchy)
				gauges.SetActive(true);

			if(customizationMenu && customizationMenu.activeInHierarchy)
				customizationMenu.SetActive(false);

			break;

		case DisplayType.Customization:

			if(topButtons && topButtons.activeInHierarchy)
				topButtons.SetActive(false);

			if(controllerButtons && controllerButtons.activeInHierarchy)
				controllerButtons.SetActive(false);

			if(gauges && gauges.activeInHierarchy)
				gauges.SetActive(false);

			if(customizationMenu && !customizationMenu.activeInHierarchy)
				customizationMenu.SetActive(true);

			break;

		case DisplayType.TopButtonsOnly:

			if(!topButtons.activeInHierarchy)
				topButtons.SetActive(true);

			if(controllerButtons.activeInHierarchy)
				controllerButtons.SetActive(false);

			if(gauges.activeInHierarchy)
				gauges.SetActive(false);

			if(customizationMenu.activeInHierarchy)
				customizationMenu.SetActive(false);

			break;

		case DisplayType.Off:

			if(topButtons &&topButtons.activeInHierarchy)
				topButtons.SetActive(false);

			if(controllerButtons &&controllerButtons.activeInHierarchy)
				controllerButtons.SetActive(false);

			if(gauges &&gauges.activeInHierarchy)
				gauges.SetActive(false);

			if(customizationMenu &&customizationMenu.activeInHierarchy)
				customizationMenu.SetActive(false);

			break;

		}

	}
	
	void LateUpdate () {

		if (RCC_SceneManager.Instance.activePlayerVehicle)
		{

			if (KMHLabel)
			{
				if (RCCSettings.units == RCC_Settings.Units.KMH)
					KMHLabel.text = inputs.KMH.ToString("0");
				else
					KMHLabel.text = (inputs.KMH * 0.62f).ToString("0");
			}
		}
	}

	public void SetDisplayType(DisplayType _displayType){

		displayType = _displayType;

	}

	void OnDisable(){

		RCC_SceneManager.OnControllerChanged -= CheckController;

	}

}

﻿using ColossalFramework.UI;
using UnityEngine;

namespace AdvancedBuildingsEditor
{
    public class Panel : UIPanel
    {
        private UIPanel m_SelectRef;

        public override void Start()
        {
            base.Start();
            this.name = "AdvancedBuildingsEditor";
            this.width = 230f;
            this.height = 222f;
            this.backgroundSprite = "MenuPanel2";
            this.canFocus = true;
            this.isInteractive = true;
            this.isVisible = true;
            this.relativePosition = new Vector3(10f, 10f);
            UILabel uiLabel = this.AddUIComponent<UILabel>();
            uiLabel.name = "Title";
            uiLabel.text = "Advanced Buildings Editor";
            uiLabel.textAlignment = UIHorizontalAlignment.Center;
            uiLabel.position = new Vector3((float)((double)this.width / 2.0 - (double)uiLabel.width / 2.0), (float)((double)uiLabel.height / 2.0 - 20.0));
            UIPanel uiPanel1 = this.AddUIComponent<UIPanel>();
            uiPanel1.anchor = UIAnchorStyle.Top | UIAnchorStyle.Left | UIAnchorStyle.Right;
            uiPanel1.transform.localPosition = Vector3.zero;
            uiPanel1.width = this.width;
            uiPanel1.height = this.height - 50f;
            uiPanel1.autoLayout = true;
            uiPanel1.autoLayoutDirection = LayoutDirection.Vertical;
            uiPanel1.autoLayoutPadding = new RectOffset(0, 0, 0, 1);
            uiPanel1.autoLayoutStart = LayoutStart.TopLeft;
            uiPanel1.relativePosition = new Vector3(6f, 50f);
            UIPanel uiPanel2 = uiPanel1.AddUIComponent<UIPanel>();
            string str1 = "SelectionPanel";
            uiPanel2.name = str1;
            double num1 = (double)uiPanel1.width;
            uiPanel2.width = (float)num1;
            double num2 = 30.0;
            uiPanel2.height = (float)num2;
            int num3 = 1;
            uiPanel2.autoLayout = num3 != 0;
            int num4 = 0;
            uiPanel2.autoLayoutDirection = (LayoutDirection)num4;
            RectOffset rectOffset1 = new RectOffset(0, 5, 0, 0);
            uiPanel2.autoLayoutPadding = rectOffset1;

            var bcButton = UIUtil.CreateButton(this, "Bulldoze Ped. Connections");
            bcButton.relativePosition = new Vector3(5, 40);
            bcButton.eventClick +=
            (comp, param) =>
            {
                Scripts.BulldozePedestrianConnections();
            };
            var seButton = UIUtil.CreateButton(this, "Make All Segments Editable");
            seButton.relativePosition = new Vector3(5, 66);
            seButton.eventClick += (comp, param) =>
            {
                Scripts.MakeAllSegmentsEditable();
            };

            var clearSpecialPointsButton = UIUtil.CreateButton(this, "Clear special points");
            clearSpecialPointsButton.relativePosition = new Vector3(5, 92);
            clearSpecialPointsButton.eventClicked += (component, param) =>
            {
                Scripts.ClearProps(true);
            };

            var clearPropsButton = UIUtil.CreateButton(this, "Clear props");
            clearPropsButton.relativePosition = new Vector3(5, 118);
            clearPropsButton.eventClicked += (component, param) =>
            {
                Scripts.ClearProps();
            };

            var reloadDecorationsButton = UIUtil.CreateButton(this, "Reload Decorations");
            reloadDecorationsButton.relativePosition = new Vector3(5, 144);
            reloadDecorationsButton.eventClicked += (component, param) =>
            {
                BuildingDecoration.ClearDecorations();
                BuildingDecoration.LoadDecorations((BuildingInfo)ToolsModifierControl.toolController.m_editPrefabInfo);
            };

            var autoPlaceSpecialPointsButton = UIUtil.CreateButton(this, "Auto-place spawn points");
            autoPlaceSpecialPointsButton.relativePosition = new Vector3(5, 170);
            autoPlaceSpecialPointsButton.eventClicked += (component, param) =>
            {
                Scripts.AutoPlaceSpecialPoints();
            };

            var addSubBuildingButton = UIUtil.CreateButton(this, "Add sub-building");
            addSubBuildingButton.relativePosition = new Vector3(5, 196);
            addSubBuildingButton.eventClicked += (component, param) =>
            {
                if (ToolsModifierControl.GetCurrentTool<DefaultTool>() == null)
                {
                    return;
                }
                if (m_SelectRef.isVisible)
                    return;
                var buildingInfo = ToolsModifierControl.toolController.m_editPrefabInfo as BuildingInfo;
                if (buildingInfo == null)
                    return;
                var component1 = m_SelectRef.GetComponent<AssetImporterAssetTemplate>();
                var callbackDelegate = new AssetImporterAssetTemplate.ReferenceCallbackDelegate(
                    info =>
                    {
                        ToolsModifierControl.SetTool<BuildingTool>();
                        ToolsModifierControl.GetTool<BuildingTool>().m_prefab = (BuildingInfo)info;
                        m_SelectRef.isVisible = false;
                    });
                component1.ReferenceCallback = callbackDelegate;
                component1.Reset();
                component1.RefreshWithFilter(AssetImporterAssetTemplate.Filter.Buildings);
                m_SelectRef.isVisible = true;
            };
            this.m_SelectRef = GameObject.Find("SelectReference").GetComponent<UIPanel>();
            this.m_SelectRef.isVisible = false;
        }

        public void Update()
        {
            isVisible = ToolsModifierControl.toolController.m_editPrefabInfo is BuildingInfo;
            if (!isVisible)
            {
                this.m_SelectRef.isVisible = false;
            }
            if (isVisible)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    if (ToolsModifierControl.GetCurrentTool<BuildingTool>() == null)
                    {
                        return;
                    }
                    ToolsModifierControl.SetTool<DefaultTool>();
                    ToolsModifierControl.GetTool<BuildingTool>().m_prefab = null;
                }
            }
        }
    }
}

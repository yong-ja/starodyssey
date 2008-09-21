using AvengersUtd.MultiversalRuleSystem.Properties;
using AvengersUtd.MultiversalRuleSystem.Space.CelestialObjects;
using AvengersUtd.Odyssey.UserInterface.RenderableControls;
using AvengersUtd.Odyssey.UserInterface.Style;
using SlimDX;
using AvengersUtd.MultiversalRuleSystem;

namespace AvengersUtd.StarOdyssey.Objects.UI
{
    public class PlanetInfoPanel : Panel, ISpriteControl
    {
        public const string ControlTag = "PlanetInfoPanel";
        const string planetInfoNameTag = "PlanetInfoName";
        const string planetInfoTitleTag = "PlanetInfoTitle";
        const string planetInfoHeaderTag = "PlanetInfoHeader";
        const string planetInfoValueTag = "PlanetInfoValue";
        const string planetInfoTableTag = "PlanetInfoTable";
        const string planetInfoTableRowTag = "PlanetInfoTableRow";

        Label planetName;
        Label planetTitle;
        Table table;

        Planet dataSource;

        TableCell habitabilityValue;
        TableCell climateValue;
        TableCell sizeValue;
        TableCell temperatureValue;
        TableCell atmDensityValue;
        TableCell compositionValue;
        TableCell gravityValue;
        TableCell densityValue;

        public Planet DataSource
        {
            get { return dataSource; }
            set
            {
                if (dataSource != value)
                {
                    dataSource = value;
                    UpdateData();
                }
            }
        }

        void UpdateData()
        {
            planetName.Text = dataSource.Name;
            planetTitle.Text = dataSource.Title;
            PlanetaryFeatures pFeatures = dataSource.PlanetaryFeatures;

            sizeValue.Text = CustomTranslator.GetSize(pFeatures.Size);
            climateValue.Text = CustomTranslator.GetClimate(pFeatures.Climate);
            temperatureValue.Text = CustomTranslator.GetTemperature(pFeatures.Temperature);
            atmDensityValue.Text = CustomTranslator.GetAtmosphericDensity(pFeatures.AtmosphericDensity);
            compositionValue.Text = CustomTranslator.GetComposition(pFeatures.Composition);
            gravityValue.Text = CustomTranslator.GetGravity(pFeatures.Gravity);
            densityValue.Text = CustomTranslator.GetDensity(pFeatures.Density);
        }

        public PlanetInfoPanel()
        {
            ApplyControlStyle(StyleManager.GetControlStyle(ControlTag));
            planetName = new Label
                             {
                                 Id = string.Format("{0}_NameLabel", ControlTag),
                                 TextStyleClass = planetInfoNameTag
                             };

            planetTitle = new Label
                              {
                                  Id = string.Format("{0}_TitleLabel", ControlTag),
                                  TextStyleClass = planetInfoTitleTag,
                                  Position = new Vector2(0, 30)
                              };

            table = new Table
                        {
                            Id = string.Format("{0}_Table", ControlTag),
                            Position = new Vector2(15, 50),
                            ControlStyleClass = planetInfoTableTag
                        };

            TableCell habitabilityHeader = new TableCell { Text = Stringtable.PS_Habitability };
            TableCell sizeHeader = new TableCell { Text = Stringtable.SS_Size };
            TableCell climateHeader = new TableCell { Text = Stringtable.SS_Climate };
            TableCell temperatureHeader = new TableCell { Text = Stringtable.SS_Temperature };
            TableCell atmDensityHeader = new TableCell { Text = Stringtable.SS_AtmosphericDensity };
            TableCell compositionHeader = new TableCell { Text = Stringtable.SS_Composition };
            TableCell gravityHeader = new TableCell { Text = Stringtable.SS_Gravity };
            TableCell densityHeader = new TableCell { Text = Stringtable.SS_Density };

            habitabilityHeader.TextStyleClass = sizeHeader.TextStyleClass =
            climateHeader.TextStyleClass = temperatureHeader.TextStyleClass =
               atmDensityHeader.TextStyleClass =
               compositionHeader.TextStyleClass =
               gravityHeader.TextStyleClass =
               densityHeader.TextStyleClass =
               planetInfoHeaderTag;

            habitabilityValue = new TableCell();
            sizeValue = new TableCell();
            climateValue = new TableCell();
            temperatureValue = new TableCell();
            atmDensityValue = new TableCell();
            compositionValue = new TableCell();
            gravityValue = new TableCell();
            densityValue = new TableCell();

            habitabilityValue.ControlStyleClass = sizeValue.ControlStyleClass =
            climateValue.ControlStyleClass = temperatureValue.ControlStyleClass =
               atmDensityValue.ControlStyleClass =
               compositionValue.ControlStyleClass =
               gravityValue.ControlStyleClass =
               densityValue.ControlStyleClass =
               ControlStyle.EmptyStyle.Name;

            habitabilityValue.TextStyleClass = sizeValue.TextStyleClass =
            climateValue.TextStyleClass = temperatureValue.TextStyleClass =
               atmDensityValue.TextStyleClass =
               compositionValue.TextStyleClass =
               gravityValue.TextStyleClass =
               densityValue.TextStyleClass =
               planetInfoValueTag;

            TableRow habitabilityRow = new TableRow();
            TableRow sizeRow = new TableRow();
            TableRow climateRow = new TableRow();
            TableRow temperatureRow = new TableRow();
            TableRow atmDensityRow = new TableRow();
            TableRow compositionRow = new TableRow();
            TableRow gravityRow = new TableRow();
            TableRow densityRow = new TableRow();

            habitabilityRow.ControlStyleClass = sizeRow.ControlStyleClass =
                climateRow.ControlStyleClass = temperatureRow.ControlStyleClass =
               atmDensityRow.ControlStyleClass =
               compositionRow.ControlStyleClass =
               gravityRow.ControlStyleClass =
               densityRow.ControlStyleClass =
               planetInfoTableRowTag;

            habitabilityRow.Cells.Add(habitabilityHeader);
            habitabilityRow.Cells.Add(habitabilityValue);
            sizeRow.Cells.Add(sizeHeader);
            sizeRow.Cells.Add(sizeValue);
            climateRow.Cells.Add(climateHeader);
            climateRow.Cells.Add(climateValue);
            temperatureRow.Cells.Add(temperatureHeader);
            temperatureRow.Cells.Add(temperatureValue);
            atmDensityRow.Cells.Add(atmDensityHeader);
            atmDensityRow.Cells.Add(atmDensityValue);
            compositionRow.Cells.Add(compositionHeader);
            compositionRow.Cells.Add(compositionValue);
            gravityRow.Cells.Add(gravityHeader);
            gravityRow.Cells.Add(gravityValue);
            densityRow.Cells.Add(densityHeader);
            densityRow.Cells.Add(densityValue);

            Add(planetName);
            Add(planetTitle);
            Add(table);

            table.Rows.Add(habitabilityRow);
            table.Rows.Add(sizeRow);
            table.Rows.Add(climateRow);
            table.Rows.Add(temperatureRow);
            table.Rows.Add(atmDensityRow);
            table.Rows.Add(compositionRow);
            table.Rows.Add(gravityRow);
            table.Rows.Add(densityRow);

        }

        protected override void UpdatePositionDependantParameters()
        {
            planetName.ComputeAbsolutePosition();
            planetTitle.ComputeAbsolutePosition();
        }

        #region ISpriteControl Members

        public void Render()
        {
            planetName.Render();
            planetTitle.Render();
        }

        #endregion
    }
}
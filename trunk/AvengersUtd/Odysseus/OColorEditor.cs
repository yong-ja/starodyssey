using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using AvengersUtd.Odyssey;
using AvengersUtd.Odyssey.UserInterface;
using AvengersUtd.Odyssey.UserInterface.Style;

namespace AvengersUtd.Odysseus
{
    public class OColorEditor : UITypeEditor
    {
        private IWindowsFormsEditorService service;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            // This tells it to show the [...] button which is clickable firing off EditValue below.
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
                service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (service != null)
            {
                // This is the code you want to run when the [...] is clicked and after it has been verified.

                // Get our currently selected color.
                OColor color = ((OColor) value);

                // Create a new instance of the ColorDialog.
                ColorChooser cc = new ColorChooser {Color = Color.FromArgb(color.GetARGB())};
                //CustomColorDialog selectionControl = new CustomColorDialog(Main.FormInstance.Handle)
                //                                         {InitialColor = Color.FromArgb(color.GetARGB())};

                // Set the selected color in the dialog.
                //selectionControl.SelectedColor

                // Show the dialog.
                cc.Show();

                // Return the newly selected color.
                value = new OColor(cc.Color.ToArgb());
            }

            return value;
        }
    }
}
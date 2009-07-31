using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using AvengersUtd.Odyssey.UserInterface.Helpers;


namespace AvengersUtd.Odyssey.UserInterface.Style
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
                CustomColorDialog selectionControl = new CustomColorDialog(OdysseyUI.Owner.Handle);

                // Set the selected color in the dialog.
                selectionControl.IntialColor = Color.FromArgb(color.GetARGB());
                
                
                // Show the dialog.
                selectionControl.Show();

                // Return the newly selected color.
                value = new OColor(selectionControl.SelectedColor.ToArgb());
            }

            return value;
        }
    }
}
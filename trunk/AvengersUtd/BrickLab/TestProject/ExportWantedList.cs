using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AvengersUtd.BrickLab;
using AvengersUtd.BrickLab.DataAccess;
using AvengersUtd.BrickLab.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class ExportWantedList
    {
        [TestMethod]
        public void TestMethod1()
        {
            Global.Init();
            InventoryViewModel inventory = new InventoryViewModel();
            inventory.DownloadSet("8028-1");
            inventory.ExportToWantedList("245700");
        }
    }
}

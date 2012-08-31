using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace AvengersUtd
{
    public class Part
    {
        public string ItemNr { get; private set; }
        public Uri ImageUri { get; private set; }
        public int Quantity { get; private set; }
        public string Description { get; private set; }

        public Part(string imageUrl, string itemNr, int quantity, string description)
        {
            Contract.Requires(Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute));
            ItemNr = itemNr;
            ImageUri = new Uri(imageUrl);
            Quantity = quantity;
            Description = description;
        }
    }
}

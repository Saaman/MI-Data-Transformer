using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIProgram.Core.Model;
using MIProgram.Core.Extensions;

namespace MIProgram.Core.AlbumImpl.LocalRepositories
{
    public static class AlbumLabelsRepository
    {
        private static readonly IList<LabelVendor> _labelVendors = new List<LabelVendor>();

        public static IList<LabelVendor> Repo
        {
            get { return _labelVendors; }
        }

        public static LabelVendor CreateOrUpdate(string label, string vendor, DateTime lastUpdate)
        {
            if (string.IsNullOrEmpty(label))
            {
                throw new ArgumentNullException("label");
            }

            var lb = label.Trim();
            var vd = vendor.Trim();

            var existingLb = _labelVendors.Where(x => x.Label.ToUpperInvariant() == lb.ToUpperInvariant()).SingleOrDefault();
            if (existingLb == null)
            {
                var labelVendor = new LabelVendor(lb, vd, lastUpdate);
                _labelVendors.Add(labelVendor);
                return labelVendor;
            }

            existingLb.UpdateVendor(vd, lastUpdate);
            return  existingLb;

        }
    }
}

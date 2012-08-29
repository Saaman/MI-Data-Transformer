using System;
using System.Collections.Generic;
using System.Linq;
using MIProgram.Core.Helpers;
using MIProgram.Core.Model;

namespace MIProgram.Core.AlbumImpl.LocalRepositories
{
    public static class AlbumLabelsRepository
    {
        private static readonly IList<LabelVendor> LabelVendors = new List<LabelVendor>();
        private static readonly IDGenerator AlbumLabelsIdGenerator = new IDGenerator();

        public static IList<LabelVendor> Repo
        {
            get { return LabelVendors; }
        }

        public static LabelVendor CreateOrUpdate(string label, string vendor, DateTime lastUpdate)
        {
            if (string.IsNullOrEmpty(label))
            {
                throw new ArgumentNullException("label");
            }

            var lb = label.Trim();
            var vd = vendor.Trim();

            var existingLb = LabelVendors.Where(x => x.Label.ToUpperInvariant() == lb.ToUpperInvariant()).SingleOrDefault();
            if (existingLb == null)
            {
                var labelVendor = new LabelVendor(AlbumLabelsIdGenerator.NewID(), lb, vd, lastUpdate);
                LabelVendors.Add(labelVendor);
                return labelVendor;
            }

            existingLb.UpdateVendor(vd, lastUpdate);
            return  existingLb;

        }
    }
}

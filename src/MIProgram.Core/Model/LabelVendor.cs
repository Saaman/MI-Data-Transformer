using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MIProgram.Core.Extensions;

namespace MIProgram.Core.Model
{
    public class LabelVendor
    {
        public string Label { get; private set; }
        public string Vendor { get; private set; }
        public DateTime _lastUpdate;

        public LabelVendor(string label, string vendor, DateTime lastUpdate)
        {
            #region Parameters validation

            if (string.IsNullOrEmpty(label))
            {
                throw new ArgumentNullException("label");
            }

            if (lastUpdate == default(DateTime))
            {
                throw new ArgumentNullException("lastUpdate");
            }

            #endregion

            Label = label;
            Vendor = vendor;
            _lastUpdate = lastUpdate;
        }

        public void UpdateVendor(string vendor, DateTime lastUpdate)
        {
            if (lastUpdate < _lastUpdate)
            {
                return;
            }

            if (string.IsNullOrEmpty(vendor))
            {
                return;
            }

            _lastUpdate = lastUpdate;
            Vendor = vendor;
        }

        public string ToSQLInsert()
        {
            string parentsString = string.Empty;
            return string.Format("INSERT INTO `mi_label_vendor` (`label`, `vendor`) VALUES ('{0}', '{1}');", Label.ToCamelCase()
                    , string.IsNullOrEmpty(Vendor) ? string.Empty : Vendor.ToCamelCase());
        }

        #region IEquality members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(LabelVendor)) return false;
            return Equals((LabelVendor)obj);
        }

        public bool Equals(LabelVendor obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.Label, Label) && Equals(obj.Vendor, Vendor) && Equals(obj._lastUpdate, _lastUpdate);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + (Label != null ? Label.GetHashCode() : 0);
                result = result * 23 + (Vendor != null ? Vendor.GetHashCode() : 0);
                result = result * 23 + _lastUpdate.GetHashCode();
                return result;
            }
        }

        #endregion
    }
}

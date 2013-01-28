using System;
using System.Text;
using MIProgram.Core.Extensions;

namespace MIProgram.Core.Model
{
    public class LabelVendor
    {
        public int Id { get; set; }
        public string Label { get; private set; }
        public string Vendor { get; private set; }
        public DateTime _lastUpdate;

        public const string RailsModelName = "music_label";
        public const string RailsModelClassName = "MusicLabel";

        public LabelVendor(int labelId, string label, string vendor, DateTime lastUpdate)
        {
            #region Parameters validation
            if (labelId == 0)
            {
                throw new ArgumentException("Reviewer creation : 'Id' cannot be null");
            }
            
            if (string.IsNullOrEmpty(label))
            {
                throw new ArgumentNullException("label");
            }

            if (lastUpdate == default(DateTime))
            {
                throw new ArgumentNullException("lastUpdate");
            }

            #endregion

            Id = labelId;
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
            return string.Format("INSERT INTO `mi_label_vendor` (`label`, `vendor`) VALUES ('{0}', '{1}');", Label.ToCamelCase()
                    , string.IsNullOrEmpty(Vendor) ? string.Empty : Vendor.ToCamelCase());
        }

        public string ToRailsInsert()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} = {1}.new", RailsModelName, RailsModelClassName);
            sb.AppendLine();
            sb.AppendFormat("{0}.assign_attributes({{id: {1}, name: '{2}', distributor: '{3}'}}, :without_protection => true)", RailsModelName, Id, Label.GetSafeRails(), Vendor);
            sb.AppendLine();
            sb.AppendFormat("{0}s << {0}", RailsModelName);
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }

        public string ToYAMLInsert()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("id: {0}", Id);
            sb.AppendLine();
            sb.AppendLine("model: music_label");
            sb.AppendFormat("name: {0}", Label.GetSafeRails());
            sb.AppendLine();
            sb.AppendFormat("distributor: {0}", Vendor);
            sb.AppendLine();
            sb.AppendFormat("created_at: {0}", _lastUpdate);
            sb.AppendLine();
            sb.AppendFormat("updated_at: {0}", _lastUpdate);
            sb.AppendLine();
            return sb.ToString();
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

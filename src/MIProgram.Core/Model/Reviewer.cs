using System;
using System.Text;
using MIProgram.Core.Extensions;

namespace MIProgram.Core.Model
{
    public class Reviewer
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Id { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }

        public Reviewer(int userId, string name, string mailAddress, DateTime lastUpdate)
        {
            #region parameters validation

            if (userId == 0)
            { throw new ArgumentException("Reviewer creation : 'Id' cannot be null"); }
            if (string.IsNullOrEmpty(name))
            { throw new ArgumentException("Reviewer creation : 'name' cannot be null nor empty"); }
            if (string.IsNullOrEmpty(mailAddress))
            { throw new ArgumentException("Reviewer creation : 'mailAddress' cannot be null nor empty"); }
            if (lastUpdate == DateTime.MinValue)
            { throw new ArgumentException("Reviewer creation : 'lastUpdate' must be a valid date"); }

            #endregion

            Name = name;
            MailAddress = mailAddress;
            LastUpdate = lastUpdate;
            CreationDate = LastUpdate;
            Id = userId;
            Password = name + Id;
        }

        public void UpdateInfos(string name, string mailAddress, DateTime? lastUpdate)
        {
            //if older version than the current one, return without doing anything
            if (lastUpdate < LastUpdate)
            {
                return;
            }

            if (!string.IsNullOrEmpty(name))
            {
                Name = name;
            }

            if (!string.IsNullOrEmpty(mailAddress))
            {
                MailAddress = mailAddress;
            }
        }

        public string ToSQLInsert()
        {
            return string.Format("INSERT INTO `{0}` (`reviewer_id`, `name`, `creation_date`, `mail`, `password`) VALUES ('{1}',  '{2}',  '{3}',  '{4}', '{5}');",
                SQLTableName, Id, Name, CreationDate.ToUnixTimeStamp(), MailAddress, Password);
        }

        public string ToRailsInsert()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{5} = {0}.new(pseudo: '{1}', email: '{2}', email_confirmation: '{2}', password: '{3}', created_at: '{4}')",
                RailsModelName.ToCamelCase(), Name, MailAddress, Password, CreationDate.ToUnixTimeStamp(), RailsModelName);
            sb.AppendLine();
            sb.AppendFormat("{0}.id = {1}", RailsModelName, Id);
            sb.AppendLine();
            sb.AppendFormat("{0}.skip_confirmation!", RailsModelName);
            sb.AppendLine();
            sb.AppendFormat("{0}s << {0}", RailsModelName);
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }

        public const string SQLTableName = "mi_accounts";
        public const string RailsModelName = "user";
    }
}
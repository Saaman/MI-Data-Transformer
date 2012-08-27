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
            while (Name.Length < 4)
            {
                Name += name;
            }
            MailAddress = name + mailAddress;
            LastUpdate = lastUpdate;
            CreationDate = LastUpdate;
            Id = userId;
            Password = name + Id;
            while(Password.Length < 7)
            {
                Password += Id;
            }
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
                while (Name.Length < 4)
                {
                    Name += name;
                }
            }

            if (!string.IsNullOrEmpty(mailAddress))
            {
                MailAddress = name + mailAddress;
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
            sb.AppendFormat("{0} = {1}.new", RailsModelName, RailsModelName.ToCamelCase());
            sb.AppendLine();
            sb.AppendFormat("{0}.assign_attributes({{id: {1}, pseudo: '{2}', email: '{3}', password: '{4}', created_at: DateTime.parse('{5}'), updated_at: DateTime.parse('{6}')}}, :without_protection => true)",
                RailsModelName, Id, Name, MailAddress, Password, CreationDate, LastUpdate);
            sb.AppendLine();
            sb.AppendFormat("{0}.email_confirmation = {0}.email", RailsModelName);
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
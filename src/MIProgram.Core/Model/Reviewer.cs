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

        public string ToYAMLInsert()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("  - id: {0}", Id);
            sb.AppendLine();
            sb.AppendFormat("    pseudo: {0}", Name);
            sb.AppendLine();
            sb.AppendFormat("    email: {0}", MailAddress.GetSafeRails(true));
            sb.AppendLine();
            sb.AppendFormat("    password: {0}", Password);
            sb.AppendLine();
            sb.AppendFormat("    created_at: {0}", CreationDate);
            sb.AppendLine();
            sb.AppendFormat("    updated_at: {0}", LastUpdate);
            sb.AppendLine();
            sb.AppendFormat("    role: :staff");
            sb.AppendLine();
            return sb.ToString();
        }

        public const string SQLTableName = "mi_accounts";
        public const string RailsPluralizedModelName = "users";
    }
}
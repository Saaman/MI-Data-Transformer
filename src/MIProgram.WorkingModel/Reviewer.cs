using System;

namespace MIProgram.WorkingModel
{
    public class Reviewer
    {
        public string Name { get; private set; }
        public string MailAddress { get; private set; }
        public DateTime LastUpdate {get; private set; }
        public DateTime CreateDate { get; private set; }
        public int UserId { get; private set; }

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
            CreateDate = LastUpdate;
            UserId = userId;
        }

        public void UpdateInfos(string name, string mailAddress, DateTime lastUpdate)
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
    }
}

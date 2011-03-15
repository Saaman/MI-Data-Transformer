using System.Xml.Serialization;

namespace MIProgram.DataAccess
{
    [XmlRoot("mi_reviews")]
    public class MIXmlRecord
    {
        [XmlElement("pn_id")]
        public string Id { get; set; }
        [XmlElement("pn_date")]
        public string CreationDate { get; set; }
        [XmlElement("pn_title")]
        public string Title { get; set; }
        [XmlElement("pn_text")]
        public string Text { get; set; }
        [XmlElement("pn_reviewer")]
        public string ReviewerName { get; set; }
        [XmlElement("pn_email")]
        public string ReviewerMail { get; set; }
        [XmlElement("pn_score")]
        public string Score { get; set; }
        [XmlElement("pn_cover")]
        public string CoverFileName { get; set; }
        [XmlElement("pn_url")]
        public string OfficialUrl { get; set; }
        [XmlElement("pn_url_title")]
        public string OfficialUrlTitle { get; set; }
        [XmlElement("pn_hits")]
        public string Hits { get; set; }

        private MIXmlRecord() { }
    }
}
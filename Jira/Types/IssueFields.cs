namespace Pit.Jira.Types
{
    public class IssueFields
    {
        public string Summary { get; set; }
        public NamedProperty IssueType { get; set; }
        public NamedProperty Resolution { get; set; }
        public NamedProperty Status { get; set; }
        public NamedProperty[] Components { get; set; }
        public JiraUser Reporter { get; set; }
        public JiraUser Assignee { get; set; }

        public Issue Parent { get; set; }
    }
}
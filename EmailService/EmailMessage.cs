namespace EmailService
{
    public class EmailMessage
    {
        public string Subject { get; set; }

        public string Content { get; set; }

        public List<string> To { get; set; }

        public EmailMessage(string subject, string content, params string[] to)
        {
            Subject = subject;
            Content = content;
            To = to.ToList();
        }
    }
}

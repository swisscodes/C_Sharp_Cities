﻿namespace CityInfo.Services
{
    public class CloudMailService : IMailService
    {
        private string _mailTo = "admin@mycompany.com";
    private string _mailFrom = "noreply@mycompany.com";

    public void Send(string subject, string message)
    {
        Console.WriteLine(
         $"""
                Mail from {_mailFrom} to {_mailTo}, with {nameof(CloudMailService)}.
                Subject: {subject}
                Message: {message}
             """);
    }
}
   
}

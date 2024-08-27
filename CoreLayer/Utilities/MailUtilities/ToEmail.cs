﻿namespace CoreLayer.Utilities.MailUtilities;

public class ToEmail
{
    public string Email { get; set; }
    public string FullName { get; set; }

    public ToEmail()
    {
        Email = string.Empty;
        FullName = string.Empty;
    }

    public ToEmail(string email, string fullName)
    {
        Email = email;
        FullName = fullName;
    }
}

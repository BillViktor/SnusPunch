namespace SnusPunch.Services.Email
{
    public static class EmailHelper
    {
        public static string GetConfirmationEmail(string aConfirmationLink)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""sv"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 0;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            padding: 20px;
                            border: 1px solid #e0e0e0;
                            border-radius: 8px;
                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            text-align: center;
                        }}
                        h1 {{
                            color: #333333;
                        }}
                        p {{
                            color: #555555;
                            line-height: 1.5;
                        }}
                        a.button {{
                            display: inline-block;
                            margin-top: 10px;
                            padding: 10px 20px;
                            color: #ffffff;
                            background-color: #007BFF;
                            text-decoration: none;
                            border-radius: 5px;
                            font-size: 16px;
                        }}
                        a.button:hover {{
                            background-color: #0056b3;
                        }}
                        .footer {{
                            margin-top: 20px;
                            font-size: 14px;
                            color: #888888;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""email-container"">
                        <h1>Välkommen till SnusPunch!</h1>
                        <p>
                            Verifiera din e-postadress genom att klicka på länken nedanför
                        </p>
                        <a href=""{aConfirmationLink}"" class=""button"">Verifiera E-post</a>
                        <p>
                            Länken är giltig i en timme.
                        </p>
                        <p class=""footer"">
                            Med vänliga hälsningar,<br>
                            <strong>SnusPunch</strong>
                        </p>
                    </div>
                </body>
                </html>
                ";
        }

        public static string GetAccountDeletedByAdminEmail()
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""sv"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 0;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            padding: 20px;
                            border: 1px solid #e0e0e0;
                            border-radius: 8px;
                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            text-align: center;
                        }}
                        h1 {{
                            color: #333333;
                        }}
                        p {{
                            color: #555555;
                            line-height: 1.5;
                        }}
                        a.button {{
                            display: inline-block;
                            margin-top: 10px;
                            padding: 10px 20px;
                            color: #ffffff;
                            background-color: #007BFF;
                            text-decoration: none;
                            border-radius: 5px;
                            font-size: 16px;
                        }}
                        a.button:hover {{
                            background-color: #0056b3;
                        }}
                        .footer {{
                            margin-top: 20px;
                            font-size: 14px;
                            color: #888888;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""email-container"">
                        <h1>Ditt konto har blivit raderat</h1>
                        <p>
                            Ditt konto på SnusPunch har blivit raderat av en administratör.
                        </p>
                        <p class=""footer"">
                            Med vänliga hälsningar,<br>
                            <strong>SnusPunch</strong>
                        </p>
                    </div>
                </body>
                </html>
                ";
        }

        public static string GetAccountDeletedConfirmation()
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""sv"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 0;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            padding: 20px;
                            border: 1px solid #e0e0e0;
                            border-radius: 8px;
                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            text-align: center;
                        }}
                        h1 {{
                            color: #333333;
                        }}
                        p {{
                            color: #555555;
                            line-height: 1.5;
                        }}
                        a.button {{
                            display: inline-block;
                            margin-top: 10px;
                            padding: 10px 20px;
                            color: #ffffff;
                            background-color: #007BFF;
                            text-decoration: none;
                            border-radius: 5px;
                            font-size: 16px;
                        }}
                        a.button:hover {{
                            background-color: #0056b3;
                        }}
                        .footer {{
                            margin-top: 20px;
                            font-size: 14px;
                            color: #888888;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""email-container"">
                        <h1>Bekräftelse på kontoborttagning</h1>
                        <p>
                            Detta är en bekräftelse på att ditt konto på SnusPunch nu är raderat. Vi hoppas att få se dig igen!
                        </p>
                        <p class=""footer"">
                            Med vänliga hälsningar,<br>
                            <strong>SnusPunch</strong>
                        </p>
                    </div>
                </body>
                </html>
                ";
        }

        public static string GetResetPasswordEmail(string aResetPasswordLink)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""sv"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 0;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background: #ffffff;
                            padding: 20px;
                            border: 1px solid #e0e0e0;
                            border-radius: 8px;
                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            text-align: center;
                        }}
                        h1 {{
                            color: #333333;
                        }}
                        p {{
                            color: #555555;
                            line-height: 1.5;
                        }}
                        a.button {{
                            display: inline-block;
                            margin-top: 10px;
                            padding: 10px 20px;
                            color: #ffffff;
                            background-color: #007BFF;
                            text-decoration: none;
                            border-radius: 5px;
                            font-size: 16px;
                        }}
                        a.button:hover {{
                            background-color: #0056b3;
                        }}
                        .footer {{
                            margin-top: 20px;
                            font-size: 14px;
                            color: #888888;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""email-container"">
                        <h1>Återställ lösenord</h1>
                        <p>
                            Återställ ditt lösenord genom att klicka på knappen nedanför.
                        </p>
                        <a href=""{aResetPasswordLink}"" class=""button"">Återställ lösenord</a>
                        <p>
                            Länken är giltig i en timme.
                        </p>
                        <p class=""footer"">
                            Med vänliga hälsningar,<br>
                            <strong>SnusPunch</strong>
                        </p>
                    </div>
                </body>
                </html>
                ";
        }
    }
}

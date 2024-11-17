namespace SnusPunch.Services.Email
{
    public static class EmailHelper
    {
        public static string GetConfirmationEmail(string aConfirmationLink)
        {
            return $@"
                <h1>Välkommen till SnusPunch!</h1>
                <p>
                    Var god verifiera din e-postadress genom att klicka på knappen nedanför.
                </p>

                <a href=""{aConfirmationLink}"">Verifiera E-post</a>

                <p>
                    Kan du inte klicka på knappen ovan? Kopiera och klistra in denna länk i din webbläsare: <br/>
                    {aConfirmationLink}
                </p>
    
                <p>
                    Med vänliga hälsningar <br/>
                    SnusPunch
                </p>
                ";
        }

        public static string GetResetPasswordEmail(string aResetPasswordLink)
        {
            return $@"
                <h1>Återställ lösenord</h1>

                <p>
                    Återställ ditt lösenord genom att klicka på knappen nedanför.
                </p>

                <a href=""{aResetPasswordLink}"">Återställ lösenord</a>

                <p>
                    Med vänliga hälsningar <br/>
                    SnusPunch
                </p>
                ";
        }
    }
}

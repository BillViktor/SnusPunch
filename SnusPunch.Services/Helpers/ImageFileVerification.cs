using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using SnusPunch.Shared.Constants;
using SnusPunch.Shared.Models.ResultModel;
using System.Text.RegularExpressions;

namespace SnusPunch.Services.Helpers
{
    public static class ImageFileVerification
    {
        public static ResultModel IsValidImage(IFormFile aFormFile)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                //Kolla så filen finns
                if(aFormFile == null || aFormFile.Length == 0)
                {
                    throw new Exception("Filen är ogiltig.");
                }

                if(aFormFile.Length > AllowedImageFileTypes.ImageMaximumBytes)
                {
                    throw new Exception($"Max storlek på fotot är {AllowedImageFileTypes.ImageMaximumBytes} bytes.");
                }

                //Kolla på extension
                var sFileExtension = Path.GetExtension(aFormFile.FileName);
                if(!AllowedImageFileTypes.AllowedExtensions.Any(x => string.Equals(x, sFileExtension, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("Ogiltigt filformat.");
                }

                //Kolla på mime
                var sContentType = aFormFile.ContentType;
                if (!AllowedImageFileTypes.AllowedMimeTypes.Any(x => string.Equals(x, sContentType, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("Ogiltigt filformat.");
                }

                //Kolla på bytes
                if (!aFormFile.OpenReadStream().CanRead)
                {
                    throw new Exception("Kan inte läsa filen.");
                }

                var sBuffer = new byte[AllowedImageFileTypes.ImageMinimumBytes];
                aFormFile.OpenReadStream().Read(sBuffer, 0, AllowedImageFileTypes.ImageMinimumBytes);
                string sContent = System.Text.Encoding.UTF8.GetString(sBuffer);
                if (Regex.IsMatch(sContent, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    throw new Exception("Ogiltigt filformat.");
                }
            }
            catch(Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }
    }
}

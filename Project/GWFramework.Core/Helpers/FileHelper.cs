
namespace GW.Helpers
{

    public static class FileHelper
    {

        public static bool CheckFolder(string fisicalpath)
        {
            bool ret = true;

            try
            {

                if (!Directory.Exists(fisicalpath))
                {
                    Directory.CreateDirectory(fisicalpath);
                }

            }
            catch (Exception ex)
            {
                ret = false;
            }

            return ret;

        }

        public static string GetFileNameByTimeStamp(string prefix)
        {
            string ret = "";

            ret = prefix + "_" + DateTime.Now.Year.ToString() +
                        DateTime.Now.Month.ToString().PadLeft(2, '0') +
                        DateTime.Now.Day.ToString().PadLeft(2, '0') +
                        DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                        DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                        DateTime.Now.Second.ToString().PadLeft(2, '0');

            return ret;
        }

        public static string CopyFile(string originalfile, string destinationfile)
        {
            string ret = "OK";

            if (File.Exists(originalfile))
            {
                if (!File.Exists(destinationfile))
                {
                    try
                    {
                        File.Copy(originalfile, destinationfile);
                    }
                    catch (Exception ex)
                    {
                        ret = "Error copying file " + ex.Message;
                    }
                }
                else
                {
                    ret = "File already exists. ";
                }

            }
            else
            {
                ret = "The original file does not exist.";
            }

            return ret;
        }

        public static bool CheckExtension(string[] allowedextensions, string extension)
        {
            bool ret = false;

            foreach (string str in allowedextensions)
            {
                if ("." + str.ToUpper() == extension.ToUpper())
                {
                    ret = true;
                    break;
                }
            }

            return ret;
        }

        public static string GetFileExtension(string filename)
        {
            string ret = "";

            string[] aux = filename.Split('.');

            ret = aux[1];

            return ret;
        }


    }

     public static class ImageHelper
     {

        // 07-06-2022: removido os metodos de manipulação de imagem
        //public static bool CheckImageDimensions(Image img, Single MaxWidth, Single MaxHeight)
        //{
        //        bool ret = false;

        //        if (img != null)
        //        {                     
        //            if (!((img.PhysicalDimension.Width > MaxHeight) || (img.PhysicalDimension.Height > MaxHeight)))
        //            {
        //                ret = true;
        //            }

        //        }

        //        return ret;
        //    }

        //public static void GetImageDimensions(string filename, ref Image img, ref Single Width, ref Single Height)
        //{
        //    if (File.Exists(filename))
        //    {
        //        Stream stream = File.Open(filename, FileMode.Open);

        //        img = Image.FromStream(stream);

        //        Width = img.PhysicalDimension.Width;
        //        Height = img.PhysicalDimension.Height;
        //    }
        //}

        //public static byte[] CropImage(string filename, int Width, int Height, int X, int Y)
        //{
        //    try
        //    {
        //        using (SD.Image OriginalImage = SD.Image.FromFile(filename))
        //        {
        //            using (SD.Bitmap bmp = new SD.Bitmap(Width, Height))
        //            {
        //                bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
        //                using (SD.Graphics Graphic = SD.Graphics.FromImage(bmp))
        //                {
        //                    Graphic.SmoothingMode = SmoothingMode.AntiAlias;
        //                    Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                    Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //                    Graphic.DrawImage(OriginalImage, new SD.Rectangle(0, 0, Width, Height), X, Y, Width, Height, SD.GraphicsUnit.Pixel);
        //                    MemoryStream ms = new MemoryStream();
        //                    bmp.Save(ms, OriginalImage.RawFormat);
        //                    bmp.Dispose();
        //                    OriginalImage.Dispose();
        //                    Graphic.Dispose();
        //                    ms.Dispose();
        //                    return ms.GetBuffer();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        return null;
        //    }
        //}

        //public static bool SaveImage(string filename, byte[] filecontent)
        //{
        //    bool ret = false;

        //    try
        //    {
        //        using (MemoryStream ms = new MemoryStream(filecontent, 0, filecontent.Length))
        //        {
        //            ms.Write(filecontent, 0, filecontent.Length);
        //            using (SD.Image CroppedImage = SD.Image.FromStream(ms, true))
        //            {
        //                CroppedImage.Save(filename, CroppedImage.RawFormat);
        //                ret = true;
        //                CroppedImage.Dispose();
        //                ms.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return ret;
        //}

        public static bool SaveFile(string filename, byte[] filecontent)
        {
            bool ret = false;

            try
            {
                File.WriteAllBytes(filename, filecontent);
                ret = true; 
            }
            catch (Exception ex)
            {

            }

            return ret;
        }

     }

}

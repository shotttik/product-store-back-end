using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;

namespace WebApplication1
{
    public class Utils
    {
        public static string DataToJson(SqlDataReader data)
        {
            var dataTable = new DataTable();
            dataTable.Load(data);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dataTable);
            return JSONString;
        }

        public static void GenerateThumbnail(string thumbPath, string thumbNewPath ,int thumbWidth=165, int thumbHeight=165)
        {
            
            String imageName = Path.GetFileName(thumbPath);
            int imageHeight = thumbHeight;
            int imageWidth = thumbWidth;

            Image fullSizeImg = Image.FromFile(thumbPath);
            Image.GetThumbnailImageAbort dummyCallBack = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            Image thumbNailImage = fullSizeImg.GetThumbnailImage(imageWidth, imageHeight, dummyCallBack, IntPtr.Zero);
            thumbNailImage.Save(thumbNewPath, ImageFormat.Jpeg);
            thumbNailImage.Dispose();
            fullSizeImg.Dispose();
            

        }
        public static bool ThumbnailCallback()
        {
            return false;
        }
    }

    public static class ConfigurationHelper
    {
        public static IConfiguration config;
        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
        }
    }
}

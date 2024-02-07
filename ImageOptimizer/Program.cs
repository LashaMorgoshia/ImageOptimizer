using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using Encoder = System.Drawing.Imaging.Encoder;

namespace Utils
{
    public class Optimizer
    {

        public static void Main(string[] args)
        {
            string sourceDirectory = "";

            if (args.Length > 0)
            {
                // The first argument should be the path of the dragged file
                string draggedFilePath = args[0];

                // Get the directory path of the dragged file
                sourceDirectory = Path.GetDirectoryName(draggedFilePath);

                Console.WriteLine("Directory Path: " + sourceDirectory);
            }
            else
            {
                Console.WriteLine("No file dragged onto the application.");
                
            }

            var destinationDirectory = $"{sourceDirectory}_optimized";

            // Check if destination directory exists, if not create it
            if (!Directory.Exists(destinationDirectory))
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            // Get all JPG files in the source directory
            string[] files = Directory.GetFiles(sourceDirectory, "*.jpg");

            var i = 0;
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destinationPath = Path.Combine(destinationDirectory, fileName);

                ShrinkAndOptimizeImage(file, destinationPath);

                i++;

                Console.WriteLine(file);
                Console.Title = $"Processed {i} of {files.Length}";
            }

            Console.WriteLine("All images have been shrunk and optimized.");
            Console.ReadLine();
        }

        static void ShrinkAndOptimizeImage(string sourcePath, string destinationPath)
        {
            using (Bitmap sourceImage = new Bitmap(sourcePath))
            {
                // Set JPEG quality to 50% (adjust as needed)
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 50L);

                // Save the image with reduced size and optimized quality
                sourceImage.Save(destinationPath, jpgEncoder, encoderParams);
            }
        }

        static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }
    }
}
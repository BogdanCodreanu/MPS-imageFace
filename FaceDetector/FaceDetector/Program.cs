using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetector {
    public class Program {

        static void Main(string[] args) {
            if (!args.Any()) {
                Console.WriteLine("Image path as first argument is needed.");
                return;
            }
            if (!File.Exists(args[0])) {
                Console.WriteLine("No file exists at the given path.");
                return;
            }
            new Program().Start(args[0]);
        }


        private Rectangle GetRectFromFace(Image<Bgr, byte> grayImg, string namedCascadeFile) {
            var cascadeClassifier = new CascadeClassifier(Path.Combine(
                Environment.CurrentDirectory, $"../../../{namedCascadeFile}"));
            return cascadeClassifier.DetectMultiScale(grayImg, 1.2, 0).AvgRect() ?? new Rectangle(-1, -1, -1, -1);
        }

        private string StringRect(Rectangle rect) => $"{rect.X} {rect.Y} {rect.Width} {rect.Height}";

        public void Start(string imagePath) {
            var img = Image.FromFile(imagePath);
            var bitmap = new Bitmap(img);
            var grayImg = new Image<Bgr, byte>(bitmap);
            var faceRect = GetRectFromFace(grayImg, "haarcascade_frontalface_alt_tree.xml");
            var leftEyeRect = GetRectFromFace(grayImg, "haarcascade_lefteye_2splits.xml");
            var rightEyeRect = GetRectFromFace(grayImg, "haarcascade_righteye_2splits.xml");



            for (int i = 0; i < bitmap.Width; i++) {
                for (int j = 0; j < bitmap.Height; j++) {
                    bitmap.SetPixel(i, j, Color.FromArgb(1, bitmap.GetPixel(i, j).R, bitmap.GetPixel(i, j).R, bitmap.GetPixel(i, j).R));
                }
            }

            using (Graphics graphics = Graphics.FromImage(bitmap)) {
                using (Pen pen = new Pen(Color.Red, 1)) {
                    graphics.DrawRectangle(pen, faceRect);
                    graphics.DrawRectangle(pen, leftEyeRect);
                    graphics.DrawRectangle(pen, rightEyeRect);
                }
            }
            Console.WriteLine($"{StringRect(faceRect)} {StringRect(leftEyeRect)} {StringRect(rightEyeRect)}");
            string newPath = Path.Combine(Path.GetDirectoryName(imagePath),
                $"{Path.GetFileNameWithoutExtension(imagePath)} detected.jpg");
            bitmap.Save(newPath, ImageFormat.Jpeg);
        }
    }

    public static class Extensions {
        public static void ForEach<T>(this IEnumerable<T> range, Action<T> action) {
            foreach (var elem in range) {
                action(elem);
            }
        }

        public static Rectangle? AvgRect(this IEnumerable<Rectangle> rects) {
            if (!rects.Any()) {
                return null;
            }
            return new Rectangle((int)rects.Average(r => r.X),
                                 (int)rects.Average(r => r.Y),
                                 (int)rects.Average(r => r.Width),
                                 (int)rects.Average(r => r.Height));
        }
    }
}

using AForge;
using AForge.Imaging;
using AForge.Math.Geometry;
using AForge.Math.Metrics;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using OpenCvSharp.ImgHash;
using System.Drawing;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using Tesseract;
using static System.Net.Mime.MediaTypeNames;

namespace Puzzlesolver.Controllers
{
    public class ImageRecognition : Controller
    {
        public ImageRecognition()
        {
            
        }

        public String ReadFile(String FilePath)
        {
            Mat img = Cv2.ImRead(FilePath);
            Mat grayscaleImg = img.Clone();

            Cv2.CvtColor(img, grayscaleImg, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(grayscaleImg, grayscaleImg, 100, 250, ThresholdTypes.Triangle);

            OpenCvSharp.Point[][] contours = Cv2.FindContoursAsArray(grayscaleImg, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple);

            List<(OpenCvSharp.Point[] contour, OpenCvSharp.Point center, bool fixedY, bool fixedX)> validContours = new();

            int counter = 0;

            foreach (var contour in contours)
            {
                int x = contour[0].X;
                int y = contour[0].Y;

                OpenCvSharp.Point[] approx = Cv2.ApproxPolyDP(contour, 0.07 * Cv2.ArcLength(contour, true), true);

                if (approx.Length == 4)
                {
                    OpenCvSharp.Rect rect = Cv2.BoundingRect(contour);

                    double ratio = (double)rect.Width / rect.Height;

                    OpenCvSharp.Point[][] contourArray = new OpenCvSharp.Point[1][];

                    contourArray[0] = contour;

                    if (ratio < 0.9 || ratio > 1.1)
                    {
                        continue;
                    }

                    var distance = contour[0].DistanceTo(contour[2]);

                    if (rect.Width < 20 || rect.Height < 20)
                    {
                        continue;
                    }

                    Moments moments = Cv2.Moments(contour);

                    int cx = (int)Math.Round(moments.M10 / moments.M00);
                    int cy = (int)Math.Round(moments.M01 / moments.M00);

                    var center = new OpenCvSharp.Point(cx, cy);

                    string text = counter.ToString();

                    int baseline = 1;

                    var dw = Cv2.GetTextSize(text, HersheyFonts.HersheySimplex, 0.35, 1, out baseline);


                    //Cv2.FillPoly(img, contourArray, Scalar.LightBlue);
                    //Cv2.DrawContours(img, contourArray, -1, Scalar.White, 1);
                    center.Y = center.Y + dw.Height / 2;
                    center.X = center.X - dw.Width / 2;
                    //Cv2.PutText(img, counter.ToString(), center, HersheyFonts.HersheySimplex, 0.35, Scalar.Black, 1, LineTypes.Link8);

                    validContours.Add((contour, center, false, false));
                    counter++;
                }
            }

            //Get approx distance between Squares
            var firstContour = validContours[0];
            var minDistance = firstContour.center.DistanceTo(validContours[validContours.Count - 1].center);

            List<(int, int)> coordinates = new List<(int, int)>();

            foreach (var validContour in validContours)
            {
                if (firstContour == validContour)
                {
                    continue;
                }

                var distance = firstContour.center.DistanceTo(validContour.center);

                //Cv2.PutText(img, ((int)distance).ToString(), validContour.center, HersheyFonts.HersheySimplex, 0.35, Scalar.Red, 1, LineTypes.Link8);

                minDistance = distance < minDistance ? distance : minDistance;
            }

            foreach (var validContour in validContours)
            {
                var x = (int)Math.Round((firstContour.center.X - validContour.center.X) / minDistance);
                var y = (int)Math.Round((firstContour.center.Y - validContour.center.Y) / minDistance);

                Cv2.PutText(img, x.ToString() + ',' + y.ToString(), validContour.center, HersheyFonts.HersheySimplex, 0.35, Scalar.Red, 1, LineTypes.Link8);

                coordinates.Add((x, y));
            }

            Cv2.ImShow("image ", img);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();

            return "0";
        }
    }
}

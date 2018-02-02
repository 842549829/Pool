using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace ChinaPay.B3B.TransactionWeb.PublicClass
{
    public class Thumbnail
    {
        private int _imgWidth = 0;
        private int _imgHeight = 0;

        public int ImgWidth
        {
            get
            {
                return _imgWidth;
            }
        }

        public int ImgHeight
        {
            get
            {
                return _imgHeight;
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="borderLength">边长</param>
        /// <param name="imgPath">原图片路径</param>
        /// <returns>缩略图数据</returns>
        public byte[] MakeThumb(int borderLength, string imgPath)
        {
            try
            {
                string originalImagePath = imgPath;
                Image originalImage = Image.FromFile(originalImagePath);

                int towidth = borderLength;
                int toheight = borderLength;

                const int x = 0;
                const int y = 0;
                _imgWidth = originalImage.Width;
                _imgHeight = originalImage.Height;

                //等比缩放（不变形，如果高大按高，宽大按宽缩放）
                if (originalImage.Width / (double)towidth < originalImage.Height / (double)toheight)
                {
                    toheight = borderLength;
                    towidth = originalImage.Width * borderLength / originalImage.Height;
                }
                else
                {
                    towidth = borderLength;
                    toheight = originalImage.Height * borderLength / originalImage.Width;
                }

                //新建一个bmp图片
                Image bitmap = new Bitmap(towidth, toheight);

                //新建一个画板
                Graphics g = Graphics.FromImage(bitmap);

                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, _imgWidth, _imgHeight), GraphicsUnit.Pixel);
                var ms = new System.IO.MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] pics = ms.GetBuffer();
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
                return pics;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="borderLength">边长</param>
        /// <param name="img">图片二进制</param>
        /// <returns>缩略图数据</returns>
        public byte[] MakeThumb(int borderLength, byte[] img)
        {
            try
            {
                var ms = new System.IO.MemoryStream(img);

                Image originalImage = Image.FromStream(ms);

                int towidth = borderLength;
                int toheight = borderLength;

                const int x = 0;
                const int y = 0;
                _imgWidth = originalImage.Width;
                _imgHeight = originalImage.Height;

                //等比缩放（不变形，如果高大按高，宽大按宽缩放）
                if (originalImage.Width / (double)towidth < originalImage.Height / (double)toheight)
                {
                    toheight = borderLength;
                    towidth = originalImage.Width * borderLength / originalImage.Height;
                }
                else
                {
                    towidth = borderLength;
                    toheight = originalImage.Height * borderLength / originalImage.Width;
                }

                //新建一个bmp图片
                Image bitmap = new Bitmap(towidth, toheight);

                //新建一个画板
                Graphics g = Graphics.FromImage(bitmap);

                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), new Rectangle(x, y, _imgWidth, _imgHeight), GraphicsUnit.Pixel);
                ms = new System.IO.MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] pics = ms.GetBuffer();
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
                return pics;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
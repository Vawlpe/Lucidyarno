﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MOLE_Back;
using MOLE_Back.Libs;

namespace win
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        
        readonly GFXHandler gh = new GFXHandler(Program.args[0]);

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Setup
            float pxsize = 2f;
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.SmoothingMode = SmoothingMode.None;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.ScaleTransform(pxsize, pxsize);
            g.FillRectangle(Brushes.Gray, 0, 0, Size.Width, Size.Height);


            Color[] pal = BuildPal();
            DrawSpr(gh.tempSPR(), pal, g);

            g.ScaleTransform(1,1);
        }
        public Bitmap bmp = null;

        /// <summary>
        /// Draw a sprite to a Graphics object canvas
        /// </summary>
        /// <param name="data">Pixelmap data</param>
        /// <param name="palette">Palette data</param>
        /// <param name="g">Graphics object</param>
        public void DrawSpr( byte[] data, Color[] palette, Graphics g)
        {
            for (int i = 0; i <= 50; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    bmp = GenerateSpritesheetBMP(data, palette, (8, 8), (i, j), 16);
                    g.DrawImage(bmp, j*8, i*8);
                }
            }
        }

        /// <summary>
        /// Weird function to generate get a sprite from an imaginary spritesheet generated on the fly, this'll have to be moved around and separated eventually
        /// </summary>
        /// <param name="data">Pixelmap data</param>
        /// <param name="palette">Palette data</param>
        /// <param name="tileSize">Width and Height of each tile</param>
        /// <param name="tileId">Which tile is bein returned</param>
        /// <param name="tilesPerRow">How many tiles go per row</param>
        /// <returns></returns>
        public Bitmap GenerateSpritesheetBMP(byte[] data, Color[] palette, (int w, int h) tileSize, (int x, int y) tileId, int tilesPerRow)
        {
            int[,] spriteData = new int[tileSize.w, tileSize.h];

            for (int x = 0; x < tileSize.w; x++)
            {
                for (int y = 0; y < tileSize.h; y++)
                {
                    int i = x + (tileId.x * tileSize.w * (tilesPerRow * tileSize.w)) + tileSize.w * y + (tileId.y * tileSize.h * tileSize.w);
                    spriteData[x, y] = data[i];
                }
            }

            // Convert to bmp
            Bitmap bmp = new Bitmap( tileSize.w, tileSize.h, PixelFormat.Format24bppRgb);
            for(int i = 0; i < tileSize.w; i++)
            {
                for (int j = 0; j < tileSize.h; j++)
                {
                    bmp.SetPixel(i,j, palette[spriteData[i,j]]);
                }
            }
            return bmp;
        }

        /// <summary>
        /// Builds Mario's palette
        /// </summary>
        /// <returns></returns>
        public Color[] BuildPal()
        {
            byte[] bPal = new byte[72];
            ushort[] sPal = new ushort[36];
            for (int i = 0; i < 8; i++)
            {
                bPal[i+4] = gh.ROMHandler.ROM[0x3280 + i];
            }
            for (int i = 0; i < 20; i++)
            {
                bPal[i+12] = gh.ROMHandler.ROM[0x32C8 + i];
            }
            for (int i = 0; i < 16; i++)
            {
                try { sPal[i] = BitConverter.ToUInt16(bPal, i * 2); } catch { }
            }

            Color[] pal = new Color[sPal.Length];

            for (int i = 2; i < sPal.Length; i++)
            {
                uint[] col = gh.Pal2Col(sPal[i]);
                pal[i] = Color.FromArgb((int)col[0], (int)col[1], (int)col[2]);
            }
            pal[0] = Color.Transparent;
            var w = gh.Pal2Col(0x7FDD);
            pal[1] = Color.FromArgb((int)w[0], (int)w[1], (int)w[2]);

            /*for (int i = 0; i < 72; i++)
            {
                bPal[i] = gh.ROMHandler.ROM[0x3280 + i];
            }
            for (int i = 0; i < 36; i++)
            {
                try { sPal[i] = BitConverter.ToUInt16(bPal, i * 2); } catch { }
            }
            Color[] pal = new Color[sPal.Length];
            for (int i = 0; i < sPal.Length; i++)
            {
                uint[] col = gh.Pal2Col(sPal[i]);
                pal[i] = Color.FromArgb((int)col[0], (int)col[1], (int)col[2]);
            }
            pal[0] = Color.Transparent;*/
            return pal;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
           
            float pxsize = 14.56f;
            Graphics g = e.Graphics;
            g.ScaleTransform(pxsize, pxsize);
            g.FillRectangle(Brushes.Gray, 0, 0, Size.Width, Size.Height);

            Color[] pal = BuildPal();
            DrawSpr(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19,20,21,22,23,24,25,26,27,28,29,30,31 }, pal, g);
            
       }
    }
}

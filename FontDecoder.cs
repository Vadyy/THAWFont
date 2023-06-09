using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Reflection.Metadata;
using static FontDecoder;

public class FontDecoder
{
    public class THUG2Font
    {
        public int FileLength;
        public int NumChars;
        public int Height;
        public int HeightOverBase;
        public List<Glyph> Glyphs;
        public int TextureDataLength;
        public short TextureWidth;
        public short TextureHeight;
        public short TextureDepth;
        public byte[] Pixels;
        public Color[] Clut;
        public int NumSubTex;
        public Bitmap Bitmap;

        public class Glyph
        {
            public short BaseLine;
            public ushort ASCII;
            public ushort CharXShift;
            public ushort CharYShift;
            public ushort CharWidth;
            public ushort CharHeight;

            public Glyph(short baseLine, ushort ascii)
            {
                BaseLine = baseLine;
                ASCII = ascii;
            }
        }
    }

    public class THAWFont
    {
        public int Height;
        public int VShift;
        public int Constant;
        public byte[] AsciiTable;
        public int HighestGlyph;
        public uint OddNum;
        public uint PtPalette;
        public List<Glyph> Glyphs;
        public Color[] Palette;
        public int BadDude;
        public ushort AlwaysTwo;
        public ushort AlwaysTwenty;
        public int ClutDepthMaybe;
        public ushort OriginalWidth;
        public ushort OriginalHeight;
        public ushort ResizedWidth;
        public ushort ResizedHeight;
        public byte MipsMaybe;
        public byte UnkEight;
        public byte Ps2ColorMaybe;
        public byte BppMaybe;
        public int ColorCount;
        public int ImageWidth;
        public int ImageHeight;
        public byte[] Pixels;
        public Bitmap Bitmap;

        public class Glyph
        {
            public float X1;
            public float Y1;
            public float X2;
            public float Y2;
            public ushort Width;
            public ushort Height;
            public ushort Shift;
            public byte UnkA;
            public byte UnkB;
        }
    }

    public THAWFont ReadTHAW(string filePath)
    {
        THAWFont font = new THAWFont();
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            // Read Header
            font.Height = reader.ReadInt32();
            font.VShift = reader.ReadInt32();
            font.Constant = reader.ReadInt32();
            font.AsciiTable = reader.ReadBytes(288);
            font.HighestGlyph = font.AsciiTable.Max();
            reader.ReadBytes(10); // Skip const_junk_a, const_junk_b, const_junk_c
            font.OddNum = reader.ReadUInt32();
            reader.ReadBytes(62); // Skip skip_me
            font.PtPalette = reader.ReadUInt32();

            // Read Glyphs
            reader.BaseStream.Seek(12 + 288 + 10 + 4 + 62 + 4, SeekOrigin.Begin); // probably useless
            font.Glyphs = new List<THAWFont.Glyph>();
            for (int i = 0; i <= font.HighestGlyph; i++)
            {
                THAWFont.Glyph glyph = new THAWFont.Glyph();
                glyph.X1 = reader.ReadSingle();
                glyph.Y1 = reader.ReadSingle();
                glyph.X2 = reader.ReadSingle();
                glyph.Y2 = reader.ReadSingle();
                glyph.Width = reader.ReadUInt16();
                glyph.Height = reader.ReadUInt16();
                glyph.Shift = reader.ReadUInt16();
                glyph.UnkA = reader.ReadByte();
                glyph.UnkB = reader.ReadByte();
                font.Glyphs.Add(glyph);
            }

            // Read PaletteTable
            reader.BaseStream.Seek(font.PtPalette, SeekOrigin.Begin); // probably useless
            font.BadDude = reader.ReadInt32();
            font.AlwaysTwo = reader.ReadUInt16();
            font.AlwaysTwenty = reader.ReadUInt16();
            font.ClutDepthMaybe = reader.ReadInt32();
            font.OriginalWidth = reader.ReadUInt16();
            font.OriginalHeight = reader.ReadUInt16();
            font.ResizedWidth = reader.ReadUInt16();
            font.ResizedHeight = reader.ReadUInt16();
            font.MipsMaybe = reader.ReadByte();
            font.UnkEight = reader.ReadByte();
            font.Ps2ColorMaybe = reader.ReadByte();
            font.BppMaybe = reader.ReadByte();
            font.ColorCount = reader.ReadInt32();

            Color[] palette = new Color[font.ColorCount];
            for (int i = 0; i < font.ColorCount; i++)
            {
                byte b = reader.ReadByte();
                byte g = reader.ReadByte();
                byte r = reader.ReadByte();
                byte a = reader.ReadByte();
                palette[i] = Color.FromArgb(a, r, g, b);
            }
            font.Palette = palette;

            // Read FontImage
            int imgWidth = reader.ReadUInt16();
            int imgHeight = reader.ReadUInt16();
            font.ImageWidth = imgWidth;
            font.ImageHeight = imgHeight;
            byte[] pixels = reader.ReadBytes(imgWidth * imgHeight);

            // Create an image from the palette and bitmap data
            font.Bitmap = new Bitmap(imgWidth, imgHeight);
            for (int i = 0; i < imgHeight; i++)
            {
                for (int j = 0; j < imgWidth; j++)
                {
                    int index = pixels[i * imgWidth + j];
                    if (index >= 0 && index < palette.Length)
                    {
                        font.Bitmap.SetPixel(j, i, font.Palette[index]);
                    }
                }
            }
        }

        return font;
    }

    public THUG2Font ReadTHUG2(string filePath)
    {
        THUG2Font font = new THUG2Font();
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            font.FileLength = reader.ReadInt32();
            reader.BaseStream.Seek(4, SeekOrigin.Current); // Skip 4 bytes
            font.NumChars = reader.ReadInt32();
            Console.WriteLine(font.NumChars + " chars");
            font.Height = reader.ReadInt32();
            font.HeightOverBase = reader.ReadInt32();

            font.Glyphs = new List<THUG2Font.Glyph>();
            for (int i = 0; i < font.NumChars; i++)
            {
                short baseline = reader.ReadInt16();
                ushort ascii = reader.ReadUInt16();
                reader.BaseStream.Seek(2, SeekOrigin.Current); // Skip 2 bytes
                font.Glyphs.Add(new THUG2Font.Glyph(baseline, ascii));
            }

            font.TextureDataLength = reader.ReadInt32();
            font.TextureWidth = reader.ReadInt16();
            font.TextureHeight = reader.ReadInt16();
            font.TextureDepth = reader.ReadInt16();
            reader.BaseStream.Seek(6, SeekOrigin.Current); // Skip 6 bytes

            int pixelDataSize = font.TextureWidth * font.TextureHeight * ((font.TextureDepth + 7) / 8);
            font.Pixels = reader.ReadBytes(pixelDataSize);

            font.Clut = new Color[256];
            for (int i = 0; i < 256; i++)
            {
                byte r = reader.ReadByte();
                byte g = reader.ReadByte();
                byte b = reader.ReadByte();
                byte a = reader.ReadByte();
                font.Clut[i] = Color.FromArgb(a, r, g, b);
            }

            font.NumSubTex = reader.ReadInt32();

            font.Bitmap = new Bitmap(font.TextureWidth, font.TextureHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            foreach (var glyph in font.Glyphs)
            {
                glyph.CharXShift = reader.ReadUInt16();
                glyph.CharYShift = reader.ReadUInt16();
                glyph.CharWidth = reader.ReadUInt16();
                glyph.CharHeight = reader.ReadUInt16();
            }

            for (int y = 0; y < font.TextureHeight; y++)
            {
                for (int x = 0; x < font.TextureWidth; x++)
                {
                    int pt = y * font.TextureWidth + x;
                    font.Bitmap.SetPixel(x, y, font.Clut[font.Pixels[pt]]);
                }
            }

            return font;
        }
    }

    public void ConvertTHUG2toTHAW(string thug2Path, string thawPath, string outputPath)
    {
        var thug2Font = ReadTHUG2(thug2Path);
        var thawFont = ReadTHAW(thawPath);

        using (var writer = new BinaryWriter(File.Open(outputPath, FileMode.Create)))
        {
            // Copy the THAW header
            writer.Write(thawFont.Height);
            writer.Write(thawFont.VShift);
            writer.Write(thawFont.Constant);
            writer.Write(thawFont.AsciiTable);
            writer.Write(new byte[10]); // const_junk_a, const_junk_b, const_junk_c
            writer.Write(thawFont.OddNum);
            writer.Write(new byte[62]); // skip_me
            writer.Write(thawFont.PtPalette);

            var thug2Glyphs = thug2Font.Glyphs;
            var thawGlyphs = thawFont.Glyphs;
            Console.WriteLine("thug2Glyphs length: " + thug2Glyphs.Count);
            Console.WriteLine("thawGlyphs length: " + thawGlyphs.Count);

            int minLength = Math.Min(thug2Font.Glyphs.Count, thawFont.Glyphs.Count - 2);

            for (int i = 0; i < minLength; i++)
            {
                var thug2Glyph = thug2Font.Glyphs[i];
                var thawGlyph = thawFont.Glyphs[i];

                float x1 = ((float)thug2Glyph.CharXShift / thug2Font.TextureWidth);
                float y1 = ((float)thug2Glyph.CharYShift / thug2Font.TextureHeight);
                float x2 = ((thug2Glyph.CharXShift + thug2Glyph.CharWidth) / (float)thug2Font.TextureWidth);
                float y2 = ((thug2Glyph.CharYShift + thug2Glyph.CharHeight) / (float)thug2Font.TextureHeight);

                writer.Write(x1);                      // TopLeftX
                writer.Write(y1);                      // TopLeftY
                writer.Write(x2);                      // BottomRightX
                writer.Write(y2);                      // BottomRightY
                writer.Write(thug2Glyph.CharWidth);    // XStretch
                writer.Write(thug2Glyph.CharHeight);   // VStretch
                writer.Write(thawGlyph.Shift);         // VShift
                writer.Write(thawGlyph.UnkA);
                writer.Write(thawGlyph.UnkB);

                // Modified width/height, needs to be moved above in between if uncommented
                /*if (i > 3 && i < 8)
                {
                    ushort width = 16;
                    ushort height = 16;
                    writer.Write(width);    // XStretch
                    writer.Write(height);    // YStretch
                    Console.WriteLine("MODDED");
                 }
                 else
                 {
                    writer.Write(thug2Glyph.CharWidth);    // XStretch
                    writer.Write(thug2Glyph.CharHeight);
                    Console.WriteLine("NORMAL");
                 }*/

                /*Console.WriteLine("TopLeftX: " + x1);
                 Console.WriteLine("TopLeftY: " + y1);
                 Console.WriteLine("BottomRightX: " + x2);
                 Console.WriteLine("BottomRightY: " + y2);
                 Console.WriteLine("XStretch: " + thug2Glyph.CharWidth);
                 Console.WriteLine("YStretch: " + thug2Glyph.CharHeight);
                 Console.WriteLine("VShift: " + thug2Glyph.CharXShift);
                 Console.WriteLine("unkA: " + thawGlyph.UnkA);
                 Console.WriteLine("unkB: " + thawGlyph.UnkB);*/
            }

            // filling in the rest with thaw data, because these buttons don't exist
            for (int i = minLength; i < thawGlyphs.Count; i++)
            {
                var glyph = thawFont.Glyphs[i];
                writer.Write(glyph.X1);   // TopLeftX
                writer.Write(glyph.Y1);   // TopLeftY
                writer.Write(glyph.X2);   // BottomRightX
                writer.Write(glyph.Y2);   // BottomRightY
                writer.Write(glyph.Width);    // XStretch
                writer.Write(glyph.Height);    // YStretch
                writer.Write(glyph.Shift);    // VShift
                writer.Write(glyph.UnkA);
                writer.Write(glyph.UnkB);
            }

            // Copy the THAW palette table
            writer.Write(thawFont.BadDude);
            writer.Write(thawFont.AlwaysTwo);
            writer.Write(thawFont.AlwaysTwenty);
            writer.Write(thawFont.ClutDepthMaybe);
            writer.Write((ushort)thug2Font.TextureWidth); // originalWidth
            writer.Write((ushort)thug2Font.TextureHeight); // originalHeight
            writer.Write((ushort)thug2Font.TextureWidth); // resizedWidth
            writer.Write((ushort)thug2Font.TextureHeight); // resizedHeight
            writer.Write(thawFont.MipsMaybe);
            writer.Write(thawFont.UnkEight);
            writer.Write(thawFont.Ps2ColorMaybe);
            writer.Write(thawFont.BppMaybe);
            writer.Write(thug2Font.Clut.Length); // colorCount

            // Write the THUG2 color palette
            foreach (var color in thug2Font.Clut)
            {
                writer.Write(color.B);
                writer.Write(color.G);
                writer.Write(color.R);
                writer.Write(color.A);
            }

            // Write the THUG2 pixel data
            writer.Write((ushort)thug2Font.TextureWidth);
            writer.Write((ushort)thug2Font.TextureHeight);
            for (int i = 0; i < thug2Font.TextureHeight; i++)
            {
                for (int j = 0; j < thug2Font.TextureWidth; j++)
                {
                    writer.Write(thug2Font.Pixels[i * thug2Font.TextureWidth + j]);
                }
            }
        }
    }
}
